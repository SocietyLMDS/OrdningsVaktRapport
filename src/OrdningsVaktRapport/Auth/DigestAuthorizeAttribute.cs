using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Filters;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Auth
{
    public class DigestAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public IRepository Repository = new Repository(new Store(true));
        public string Role;

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            if (headers.Authorization != null)
            {
                var header = DigestAuthorizeUtils.ExtractHeaderValues(headers.Authorization.Parameter, actionContext.Request.Method.Method);

                if (DigestAuthorizeUtils.CheckNonce(header.Nonce, header.NonceCount))
                {
                    var user = Repository.Authenticate(header.UserName);
                    
                    if (user != null)
                    {
                        var ha1 = DigestAuthorizeUtils.ConvertStringToMd5Hash(string.Format("{0}:{1}:{2}", header.UserName, header.Realm, user.Password));
                        var ha2 = DigestAuthorizeUtils.ConvertStringToMd5Hash(string.Format("{0}:{1}", header.Method, header.Uri));
                        var computedResponse = DigestAuthorizeUtils.ConvertStringToMd5Hash(string.Format("{0}:{1}:{2}:{3}:{4}:{5}", ha1, header.Nonce, header.NonceCount, header.CNonce, header.QoP, ha2));

                        if (string.CompareOrdinal(header.Response, computedResponse) == 0)
                        {
                            var principal = SetPrincipal(user);
                            if (principal.IsInRole(Role))
                            {
                                actionContext.Request.GetRequestContext().Principal = principal;
                                return;
                            }
                        }
                    }
                }
            }

            HandleUnauthorizedRequest(actionContext);
        }

        private static GenericPrincipal SetPrincipal(IUsernamePassword currentUser)
        {
            var identity = new GenericIdentity(currentUser.Username, "Digest");

            if (currentUser.GetType() == typeof(CompanyEntity))
            {
                var company = currentUser as CompanyEntity;
                identity.AddClaim(new Claim(CustomClaims.Id, company.Id.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CompanyName, company.Name));
                identity.AddClaim(new Claim(CustomClaims.CompanyId, "null"));
                identity.AddClaim(new Claim(CustomClaims.CurrentUser, "Company"));
                identity.AddClaim(new Claim(CustomClaims.Firstname, company.ManagerFirstname ?? ""));
                identity.AddClaim(new Claim(CustomClaims.Lastname, company.ManagerLastname ?? ""));
                identity.AddClaim(new Claim(CustomClaims.EmailAddress, company.EmailAddress ?? ""));
            }
            else if (currentUser.GetType() == typeof(EmployeeEntity))
            {
                var employee = currentUser as EmployeeEntity;
                identity.AddClaim(new Claim(CustomClaims.Id, employee.Id.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CompanyName, ""));
                identity.AddClaim(new Claim(CustomClaims.CompanyId, employee.CompanyId.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CurrentUser, "Employee"));
                identity.AddClaim(new Claim(CustomClaims.Firstname, employee.Firstname));
                identity.AddClaim(new Claim(CustomClaims.Lastname, employee.Lastname ));
                identity.AddClaim(new Claim(CustomClaims.EmailAddress, employee.EmailAddress));
            }

            var role = (currentUser.GetType() == typeof(CompanyEntity)) ? new[] { "Company", "Company, Employee" } : new[] { "Employee", "Company, Employee" };
            var principal = new GenericPrincipal(identity, role);

            return principal;
        }

        private static void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Digest realm=\"OrdningsVaktsRapport\", nonce=\"" + DigestAuthorizeUtils.GenerateNonce() + "\", qop=\"auth\"");
        }
    }
}
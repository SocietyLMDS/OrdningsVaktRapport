using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using Ninject;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Auth
{
    public class BasicAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public IRepository Repository = new Repository(new Store(true));
        public string Role;

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                    return;
            }

            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var credArray = GetCredentials(authHeader);
                    var username = credArray[0];
                    var password = credArray[1];

                    try
                    {
                        var company = Repository.Authenticate(username);
                        var principal = SetPrincipal(company);
                        if (principal.IsInRole(Role))
                        {
                            actionContext.Request.GetRequestContext().Principal = principal;
                            return;
                        }
                        
                        HandleUnauthorizedRequest(actionContext);
                        
                   }
                    catch (Exception)
                    {
                        HandleUnauthorizedRequest(actionContext);
                    }
                }
            }

            HandleUnauthorizedRequest(actionContext);
        }

        private static GenericPrincipal SetPrincipal(IUsernamePassword currentUser)
        {
            var identity = new GenericIdentity(currentUser.Username, "basic");

            if (currentUser.GetType() == typeof(CompanyEntity))
            {
                var company = currentUser as CompanyEntity;
                identity.AddClaim(new Claim(CustomClaims.Id, company.Id.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CompanyName, company.Name));
                identity.AddClaim(new Claim(CustomClaims.CompanyId, "null"));
                identity.AddClaim(new Claim(CustomClaims.CurrentUser, "Company"));
                identity.AddClaim(new Claim(CustomClaims.Firstname, company.ManagerFirstname));
                identity.AddClaim(new Claim(CustomClaims.Lastname, company.ManagerLastname));
                identity.AddClaim(new Claim(CustomClaims.EmailAddress, company.EmailAddress));
            }
            else if (currentUser.GetType() == typeof(EmployeeEntity))
            {
                var employee = currentUser as EmployeeEntity;
                identity.AddClaim(new Claim(CustomClaims.Id, employee.Id.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CompanyName, ""));
                identity.AddClaim(new Claim(CustomClaims.CompanyId, employee.CompanyId.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CurrentUser, "Employee"));
                identity.AddClaim(new Claim(CustomClaims.Firstname, employee.Firstname));
                identity.AddClaim(new Claim(CustomClaims.Lastname, employee.Lastname));
                identity.AddClaim(new Claim(CustomClaims.EmailAddress, employee.EmailAddress));
            }

            var role = (currentUser.GetType() == typeof(CompanyEntity)) ? new[] { "Company", "Company, Employee" } : new[] { "Employee", "Company, Employee" };
            var principal = new GenericPrincipal(identity, role);

            return principal;
        }

        private string[] GetCredentials(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {
            //Base 64 encoded string
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));

            var credArray = cred.Split(':');

            return credArray;
        }

        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Over/Out'");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Entities;

namespace OrdningsVaktRapport.Auth
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        public const string BasicScheme = "Basic";
        public const string ChallengeAuthenticationHeaderName = "WWW-Authenticate";
        public const char AuthorizationHeaderSeparator = ':';

        private readonly IRepository _repository;

        public BasicAuthenticationMessageHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;
            
            if (authHeader == null || authHeader.Scheme != BasicScheme)
            {
                return CreateUnAuthorizedResponse();
            }

            var encodedCredentials = authHeader.Parameter;
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.ASCII.GetString(credentialBytes);
            var credentialParts = credentials.Split(AuthorizationHeaderSeparator);

            if (credentialParts.Length != 2)
            {
                return CreateUnAuthorizedResponse();
            }

            var username = credentialParts[0].Trim();
            var password = credentialParts[1].Trim();

            try
            {
                var user = _repository.Authenticate(username);
                SetPrincipal(user);
            }
            catch (Exception)
            {
                return CreateUnAuthorizedResponse();
            }

            return base.SendAsync(request, cancellationToken);
        }

        private void SetPrincipal(IUsernamePassword currentUser)
        {
            var identity = new GenericIdentity(currentUser.Username, BasicScheme);

            if (currentUser.GetType() == typeof(CompanyEntity))
            {
                var company = currentUser as CompanyEntity;
                identity.AddClaim(new Claim(CustomClaims.Id, company.Id.ToString()));
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
                identity.AddClaim(new Claim(CustomClaims.CompanyId, employee.CompanyId.ToString()));
                identity.AddClaim(new Claim(CustomClaims.CurrentUser, "Employee"));
                identity.AddClaim(new Claim(CustomClaims.Firstname, employee.Firstname));
                identity.AddClaim(new Claim(CustomClaims.Lastname, employee.Lastname));
                identity.AddClaim(new Claim(CustomClaims.EmailAddress, employee.EmailAddress));
            }

            var role = (currentUser.GetType() == typeof(CompanyEntity)) ? new[] { "Company" } : new[] { "Employee" };
            var principal = new GenericPrincipal(identity, role);
            
            Thread.CurrentPrincipal = principal;
            
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }

        }

        private Task<HttpResponseMessage> CreateUnAuthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(ChallengeAuthenticationHeaderName, BasicScheme);

            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return taskCompletionSource.Task;
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Models;

namespace OrdningsVaktRapport.Controllers
{
    public class SecurityController : ApiController
    {
        private readonly IRepository _repository;

        public SecurityController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public CurrentUser Login()
        {
            // ReSharper disable PossibleNullReferenceException
            var currentclaims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.Id).Value;
            var companyId = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.CompanyId).Value;
            var companyName = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.CompanyName).Value;
            var currentUser = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.CurrentUser).Value;
            var firstName = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.Firstname).Value;
            var lastName = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.Lastname).Value;
            var emailAddress = currentclaims.Claims.SingleOrDefault(s => s.Type == CustomClaims.EmailAddress).Value;

            var retCurrentUser = new CurrentUser
                {
                    Id = id,
                    CompanyId = companyId,
                    CompanyName = companyName,
                    UserType = currentUser,
                    Firstname = firstName,
                    Lastname = lastName,
                    EmailAddress = emailAddress
                };

            return retCurrentUser;
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public bool Logout()
        {
            var header = DigestAuthorizeUtils.ExtractHeaderValues(Request.Headers.Authorization.Parameter, Request.Method.Method);
            var response = DigestAuthorizeUtils.RemoveNonce(header.Nonce);
            return response;
        }

        [HttpPost]
        public string ForgotPassword([FromBody] EmailModel email)
        {
            try
            {
                return _repository.ForgotPassword(email.EmailAddress);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company, Employee")]
        public string ChangePassword([FromBody] CurrentUserPassword currentUser)
        {
            try
            {
                return _repository.ChangePassword(currentUser);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}

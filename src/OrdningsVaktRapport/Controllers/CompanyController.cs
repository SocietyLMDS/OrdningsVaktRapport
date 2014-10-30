using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Utils;
using Newtonsoft.Json;

namespace OrdningsVaktRapport.Controllers
{
    public class CompanyController : ApiController 
    {
        private readonly IRepository _repository;

        public CompanyController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public string AddCompany([FromBody] CompanyEntity company)
        {
            try
            {
                company.Id = Guid.NewGuid();
                return _repository.AddCompany(company);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public CompanyEntity GetCompanyById(Guid id)
        {
            
            return _repository.GetCompanyById(new CompanyEntity{Id = id});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<CompanyEntity> GetAllCompany()
        {
            return _repository.GetAllCompanies();
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifyCompany([FromBody] CompanyEntity company)
        {
            try
            {
                return _repository.ModifyCompany(company);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteCompany(Guid id)
        {
            try
            {
                return _repository.DeleteCompany(new CompanyEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}

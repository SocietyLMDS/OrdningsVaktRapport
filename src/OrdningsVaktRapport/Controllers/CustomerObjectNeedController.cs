using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Controllers
{
    public class CustomerObjectNeedController : ApiController
    {
        private readonly IRepository _repository;

        public CustomerObjectNeedController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddNeedToCustomerObject([FromBody] Need need)
        {
            try
            {
                need.Id = Guid.NewGuid();
                return _repository.AddNeedToCustomerObject(need);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public Need GetNeedFromCustomerObjectById(Guid id, Guid companyId, Guid customerId, Guid customerObjectId)
        {
            return _repository.GetNeedFromCustomerObjectById(new Need { Id = id, CompanyId = companyId, CustomerId = customerId, CustomerObjectId = customerObjectId });
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifyNeedOnCustomerObject([FromBody] Need need)
        {
            try
            {
                return _repository.ModifyNeedOnCustomerObject(need);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteNeedFromCustomerObject(Guid id, Guid companyId, Guid customerId, Guid customerObjectId)
        {
            try
            {
                return _repository.DeleteNeedFromCustomerObject(new Need { Id = id, CompanyId = companyId, CustomerId = customerId, CustomerObjectId = customerObjectId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
           
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllNeedsFromCustomerObject(Guid id, Guid companyId, Guid customerId)
        {
            try
            {
                return _repository.DeleteAllNeedsFromCustomerObject(new CustomerObject { Id = id, CompanyId = companyId, CustomerId = customerId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}

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
    public class CustomerObjectController : ApiController
    {
        private readonly IRepository _repository;

        public CustomerObjectController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddCustomerObject([FromBody] CustomerObject customerObject)
        {
            try
            {
                customerObject.Id = Guid.NewGuid();
                return _repository.AddObjectToCustomer(customerObject);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public CustomerObject GetCustomerObjectById(Guid id, Guid companyId, Guid customerId)
        {
            return _repository.GetCustomerObjectById(new CustomerObject{ Id = id, CustomerId = customerId,CompanyId = companyId});
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifyCustomerObject([FromBody] CustomerObject customerObject)
        {
            try
            {
                return _repository.ModifyCustomerObject(customerObject);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteCustomerObject(Guid id, Guid companyId, Guid customerId)
        {
            try
            {
                return _repository.DeleteCustomerObject(new CustomerObject { Id = id, CustomerId = customerId, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllCustomerObject(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteAllCustomerObject(new CustomerEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
           
        }
    }
}

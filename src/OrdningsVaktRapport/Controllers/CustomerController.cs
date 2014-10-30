using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Entities;

namespace OrdningsVaktRapport.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly IRepository _repository;

        public CustomerController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddCustomer([FromBody] CustomerEntity customer)
        {
            customer.Id = Guid.NewGuid();
            try
            {
                return _repository.AddCustomer(customer);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public CustomerEntity GetCustomerById(Guid id, Guid companyId)
        {
            return _repository.GetCustomerById(new CustomerEntity {Id = id, CompanyId = companyId});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public List<CustomerEntity> GetAllCustomer(Guid id)
        {
            return _repository.GetAllCustomer(new CompanyEntity {Id = id});
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifyCustomer([FromBody] CustomerEntity customer)
        {
            try
            {
                return _repository.ModifyCustomer(customer);
            }
            catch (Exception e)
            {
               return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteCustomer(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteCustomer(new CustomerEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
               return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllCustomer(Guid id)
        {
            try
            {
                return _repository.DeleteAllCustomer(new CompanyEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}

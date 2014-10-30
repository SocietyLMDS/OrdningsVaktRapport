using System;
using System.Collections.Generic;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Entities;

namespace OrdningsVaktRapport.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IRepository _repository;

        public EmployeeController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddEmployee([FromBody] EmployeeEntity employee)
        {
            try
            {
                employee.Id = Guid.NewGuid();
                return _repository.AddEmployee(employee);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public EmployeeEntity GetEmployeeById(Guid id, Guid companyId)
        {
            return _repository.GetEmployeeById(new EmployeeEntity {Id = id, CompanyId = companyId});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<EmployeeEntity> GetAllEmployee(Guid id)
        {
            return _repository.GetAllEmployee(new CompanyEntity { Id = id });
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company, Employee")]
        public string ModifyEmployee([FromBody] EmployeeEntity employee)
        {
            try
            {
                return _repository.ModifyEmployee(employee);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteEmployee(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteEmployee(new EmployeeEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllEmployee(Guid id)
        {
            try
            {
                return _repository.DeleteAllEmployee(new CompanyEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}

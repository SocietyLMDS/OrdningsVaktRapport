using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Utils;

namespace OrdningsVaktRapport.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly IRepository _repository;

        public ScheduleController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddSchedule([FromBody] ScheduleEntity schedule)
        {
            try
            {
                schedule.Id = Guid.NewGuid();
                return _repository.AddSchedule(schedule);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public string GetScheduleLimit()
        {
            return ConfigurationManager.AppSettings["ScheduleLimit"];
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public ScheduleEntity GetScheduleById(Guid id, Guid companyId)
        {
            return _repository.GetScheduleById(new ScheduleEntity {Id = id, CompanyId = companyId});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<ScheduleEntity> GetCustomerSchedules(Guid id, Guid companyId)
        {
            return _repository.GetCustomerSchedules(new CustomerEntity {Id = id, CompanyId = companyId});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<ScheduleEntity> GetCustomerObjectSchedules(Guid id, Guid companyId)
        {
            return _repository.GetCustomerObjectSchedules(new CustomerObject {Id = id, CompanyId = companyId});
        } 

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<ScheduleEntity> GetAllSchedules(Guid id)
        {
            return _repository.GetAllSchedules(new CompanyEntity {Id = id});
        }
 
        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteSchedule(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteSchedule(new ScheduleEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
           
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllSchedule(Guid id)
        {
            try
            {
                return _repository.DeleteAllSchedule(new CompanyEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllCustomerSchedule(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteAllCustomerSchedule(new CustomerEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllCustomerObjectSchedule(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteAllCustomerObjectSchedule(new CustomerObject { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifySchedule([FromBody] ScheduleEntity schedule)
        {
            try
            {
                return _repository.ModifySchedule(schedule);
            }
            catch (Exception e)
            {
               return e.Message;
            }
            
        }
    }
}

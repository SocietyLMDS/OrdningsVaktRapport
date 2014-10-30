using System;
using System.Collections.Generic;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Entities;

namespace OrdningsVaktRapport.Controllers
{
    public class ShiftController : ApiController
    {
        private readonly IRepository _repository;

        public ShiftController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company")]
        public string AddShiftToSchedule([FromBody] Shift shift)
        {
            try
            {
                shift.Id = Guid.NewGuid();
                return _repository.AddShiftToSchedule(shift);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public Shift GetShiftById(Guid id, Guid companyId)
        {
            return _repository.GetShiftById(new Shift { Id = id, ScheduleId = companyId });
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public List<Shift> GetAllEmployeeShifts(Guid id,Guid companyId)
        {
            return _repository.GetAllEmployeeShifts(new EmployeeEntity { Id = id, CompanyId = companyId});
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllEmployeeShiftFromSchedule(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteAllEmployeeShiftFromSchedule(new EmployeeEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteShiftFromSchedule(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteShiftFromSchedule(new Shift { Id = id, ScheduleId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllShiftFromSchedule(Guid id)
        {
            try
            {
                return _repository.DeleteAllShiftFromSchedule(new ScheduleEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpPut]
        [DigestAuthorize(Role = "Company")]
        public string ModifyShiftOnSchedule([FromBody] Shift shift)
        {
            try
            {
                return _repository.ModifyShiftOnSchedule(shift);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpPost]
        [DigestAuthorize(Role = "Employee")]
        public string SetEmployeeShiftStatus([FromBody] Shift shift)
        {
            try
            {
                return _repository.SetEmployeeShiftStatus(shift);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}

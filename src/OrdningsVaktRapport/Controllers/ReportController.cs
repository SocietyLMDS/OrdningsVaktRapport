using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OrdningsVaktRapport.Auth;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IRepository _repository;

        public ReportController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [DigestAuthorize(Role = "Company, Employee")]
        public string AddReport([FromBody] ReportEntity report)
        {
            try
            {
                report.Id = Guid.NewGuid();
                return _repository.AddReport(report);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public ReportEntity GetReportById(Guid id, Guid companyId)
        {
            return _repository.GetReportById(new ReportEntity {Id = id, CompanyId = companyId});
        } 

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<ReportEntity> GetAllReports(Guid id)
        {
            return _repository.GetAllReports(new CompanyEntity {Id = id});
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company, Employee")]
        public List<ReportEntity> GetAllReportsByEmployee(Guid id, Guid companyId)
        {
            return _repository.GetAllReportsByEmployee(new EmployeeEntity { Id = id, CompanyId = companyId });
        }

        [HttpGet]
        [DigestAuthorize(Role = "Company")]
        public List<ReportEntity> GetAllReportsByCustomer(Guid id, Guid companyid)
        {
            return _repository.GetAllReportsByCustomer(new CustomerEntity { Id = id, CompanyId = companyid });
        }
        
        [HttpPut]
        [DigestAuthorize(Role = "Company, Employee")]
        public string ModiFyReport([FromBody] ReportEntity report)
        {
            try
            {
                return _repository.ModifyReport(report);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteReport(Guid id, Guid companyId)
        {
            try
            {
                return _repository.DeleteReport(new ReportEntity { Id = id, CompanyId = companyId });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        [HttpDelete]
        [DigestAuthorize(Role = "Company")]
        public string DeleteAllReport(Guid id)
        {
            try
            {
                return _repository.DeleteAllReport(new CompanyEntity { Id = id });
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
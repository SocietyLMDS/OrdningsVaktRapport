using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Services
{
    public interface IRepository
    {
        string AddCompany(CompanyEntity company);
        CompanyEntity GetCompanyById(CompanyEntity company);
        List<CompanyEntity> GetAllCompanies();
        string DeleteCompany(CompanyEntity company);
        string ModifyCompany(CompanyEntity company);
       
        string AddEmployee(EmployeeEntity employee);
        EmployeeEntity GetEmployeeById(EmployeeEntity employee);
        List<EmployeeEntity> GetAllEmployee(CompanyEntity company);
        string DeleteEmployee(EmployeeEntity employee);
        string ModifyEmployee(EmployeeEntity employee);
        string DeleteAllEmployee(CompanyEntity company);

        string AddCustomer(CustomerEntity objectEntity);
        CustomerEntity GetCustomerById(CustomerEntity objectEntity);
        List<CustomerEntity> GetAllCustomer(CompanyEntity company);
        string DeleteCustomer(CustomerEntity objectEntity);
        string ModifyCustomer(CustomerEntity objectEntity);
        string DeleteAllCustomer(CompanyEntity company);
        string AddObjectToCustomer(CustomerObject customerObject);
        CustomerObject GetCustomerObjectById(CustomerObject customerObject);
        string ModifyCustomerObject(CustomerObject customerObject);
        string DeleteCustomerObject(CustomerObject customerObject);
        string DeleteAllCustomerObject(CustomerEntity customer);
        string AddNeedToCustomerObject(Need need);
        Need GetNeedFromCustomerObjectById(Need need);
        string ModifyNeedOnCustomerObject(Need need);
        string DeleteNeedFromCustomerObject(Need need);
        string DeleteAllNeedsFromCustomerObject(CustomerObject customerObject);

        string AddReport(ReportEntity report);
        ReportEntity GetReportById(ReportEntity report);
        List<ReportEntity> GetAllReports(CompanyEntity company);
        List<ReportEntity> GetAllReportsByEmployee(EmployeeEntity employee);
        List<ReportEntity> GetAllReportsByCustomer(CustomerEntity customer); 
        string DeleteReport(ReportEntity report);
        string ModifyReport(ReportEntity report);
        string DeleteAllReport(CompanyEntity company);

        string AddSchedule(ScheduleEntity schedule);
        ScheduleEntity GetScheduleById(ScheduleEntity schedule);
        List<ScheduleEntity> GetCustomerSchedules(CustomerEntity customer);
        List<ScheduleEntity> GetCustomerObjectSchedules(CustomerObject customerObject);
        List<ScheduleEntity> GetAllSchedules(CompanyEntity company);
        string DeleteSchedule(ScheduleEntity schedule);
        string ModifySchedule(ScheduleEntity schedule);
        string DeleteAllSchedule(CompanyEntity company);
        string DeleteAllCustomerSchedule(CustomerEntity customer);
        string DeleteAllCustomerObjectSchedule(CustomerObject customerObject);
        string AddShiftToSchedule(Shift shift);
        Shift GetShiftById(Shift shift);
        List<Shift> GetAllEmployeeShifts(EmployeeEntity employee);
        string DeleteShiftFromSchedule(Shift shift);
        string DeleteAllShiftFromSchedule(ScheduleEntity schedule);
        string ModifyShiftOnSchedule(Shift shift);
        string DeleteAllEmployeeShiftFromSchedule(EmployeeEntity employee);

        string ForgotPassword(string email);
        IUsernamePassword Authenticate(string username);
        string ChangePassword(CurrentUserPassword currentUser);
        string UpdateCompanyLogoLink(string fileLink, Guid companyId);
        string SetEmployeeShiftStatus(Shift shift);
    }
}

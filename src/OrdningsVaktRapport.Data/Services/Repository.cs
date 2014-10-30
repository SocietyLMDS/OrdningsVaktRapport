using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Services
{
    public class Repository : IRepository
    {
        public Store Store;
        
        public Repository(Store store)
        {
            Store = store;
        }

        public string AddCompany(CompanyEntity company)
        {
            return Store.AddCompany(company);
        }

        public CompanyEntity GetCompanyById(CompanyEntity company)
        {
            return Store.GetCompanyById(company);
        }

        public List<CompanyEntity> GetAllCompanies()
        {
            return Store.GetAllCompanies();
        }

        public string DeleteCompany(CompanyEntity company)
        {
            return Store.DeleteCompany(company);
        }

        public string ModifyCompany(CompanyEntity company)
        {
            return Store.ModifyCompany(company);
        }

        public string AddEmployee(EmployeeEntity employee)
        {
            return Store.AddEmployee(employee);
        }

        public EmployeeEntity GetEmployeeById(EmployeeEntity employee)
        {
            return Store.GetEmployeeById(employee);
        }

        public List<EmployeeEntity> GetAllEmployee(CompanyEntity company)
        {
            return Store.GetAllEmployee(company);
        }

        public string DeleteEmployee(EmployeeEntity employee)
        {
            return Store.DeleteEmployee(employee);
        }

        public string ModifyEmployee(EmployeeEntity employee)
        {
            return Store.ModifyEmployee(employee);
        }

        public string DeleteAllEmployee(CompanyEntity company)
        {
            return Store.DeleteAllEmployee(company);
        }

        public string AddCustomer(CustomerEntity customerEntity)
        {
            return Store.AddCustomer(customerEntity);
        }

        public CustomerEntity GetCustomerById(CustomerEntity customerEntity)
        {
            return Store.GetCustomerById(customerEntity);
        }

        public List<CustomerEntity> GetAllCustomer(CompanyEntity company)
        {
            return Store.GetAllCustomer(company);
        }

        public string DeleteCustomer(CustomerEntity customerEntity)
        {
            return Store.DeleteCustomer(customerEntity);
        }

        public string ModifyCustomer(CustomerEntity customerEntity)
        {
            return Store.ModifyCustomer(customerEntity);
        }

        public string DeleteAllCustomer(CompanyEntity company)
        {
            return Store.DeleteAllCustomer(company);
        }

        public string AddObjectToCustomer(CustomerObject customerObject)
        {
            return Store.AddObjectToCustomer(customerObject);
        }

        public CustomerObject GetCustomerObjectById(CustomerObject customerObject)
        {
            return Store.GetCustomerObjectById(customerObject);
        }

        public string ModifyCustomerObject(CustomerObject customerObject)
        {
            return Store.ModifyCustomerObject(customerObject);
        }

        public string DeleteCustomerObject(CustomerObject customerObject)
        {
            return Store.DeleteCustomerObject(customerObject);
        }

        public string DeleteAllCustomerObject(CustomerEntity customer)
        {
            return Store.DeleteAllCustomerObject(customer);
        }

        public string AddNeedToCustomerObject(Need need)
        {
            return Store.AddNeedToCustomerObject(need);
        }

        public Need GetNeedFromCustomerObjectById(Need need)
        {
            return Store.GetNeedFromCustomerObjectById(need);
        }

        public string ModifyNeedOnCustomerObject(Need need)
        {
            return Store.ModifyNeedOnCustomerObject(need);
        }

        public string DeleteNeedFromCustomerObject(Need need)
        {
            return Store.DeleteNeedFromCustomerObject(need);
        }

        public string DeleteAllNeedsFromCustomerObject(CustomerObject customerObject)
        {
            return Store.DeleteAllNeedsFromCustomerObject(customerObject);
        }

        public string AddReport(ReportEntity report)
        {
            return Store.AddReport(report);
        }

        public ReportEntity GetReportById(ReportEntity report)
        {
            return Store.GetReportById(report);
        }

        public List<ReportEntity> GetAllReports(CompanyEntity company)
        {
            return Store.GetallReports(company);
        }

        public List<ReportEntity> GetAllReportsByEmployee(EmployeeEntity employee)
        {
            return Store.GetAllReportsByEmployee(employee);
        }

        public List<ReportEntity> GetAllReportsByCustomer(CustomerEntity customer)
        {
            return Store.GetAllReportsByCustomer(customer);
        }

        public string DeleteReport(ReportEntity report)
        {
            return Store.DeleteReport(report);
        }

        public string ModifyReport(ReportEntity report)
        {
            return Store.ModifyReport(report);
        }

        public string DeleteAllReport(CompanyEntity company)
        {
            return Store.DeleteAllReport(company);
        }

        public string AddSchedule(ScheduleEntity schedule)
        {
            return Store.AddSchedudle(schedule);
        }

        public ScheduleEntity GetScheduleById(ScheduleEntity schedule)
        {
            return Store.GetScheduleById(schedule);
        }

        public List<ScheduleEntity> GetCustomerSchedules(CustomerEntity customer)
        {
            return Store.GetCustomerSchedules(customer);
        }

        public List<ScheduleEntity> GetCustomerObjectSchedules(CustomerObject customerObject)
        {
            return Store.GetCustomerObjectSchedules(customerObject);
        }

        public List<ScheduleEntity> GetAllSchedules(CompanyEntity company)
        {
            return Store.GetAllSchedules(company);
        }

        public string DeleteSchedule(ScheduleEntity schedule)
        {
            return Store.DeleteSchedule(schedule);
        }

        public string ModifySchedule(ScheduleEntity schedule)
        {
            return Store.ModifySchedule(schedule);
        }

        public string DeleteAllSchedule(CompanyEntity company)
        {
            return Store.DeleteAllSchedule(company);
        }

        public string DeleteAllCustomerSchedule(CustomerEntity customer)
        {
            return Store.DeleteAllCustomerSchedule(customer);
        }

        public string DeleteAllCustomerObjectSchedule(CustomerObject customerObject)
        {
            return Store.DeleteAllCustomerObjectSchedule(customerObject);
        }

        public string AddShiftToSchedule(Shift shift)
        {
            return Store.AddShiftToSchedule(shift);
        }

        public Shift GetShiftById(Shift shift)
        {
            return Store.GetShiftById(shift);
        }

        public List<Shift> GetAllEmployeeShifts(EmployeeEntity employee)
        {
            return Store.GetAllEmployeeShifts(employee);
        }

        public string DeleteShiftFromSchedule(Shift shift)
        {
            return Store.DeleteShiftFromSchedule(shift);
        }

        public string DeleteAllShiftFromSchedule(ScheduleEntity schedule)
        {
            return Store.DeleteAllShiftFromSchedule(schedule);
        }

        public string ModifyShiftOnSchedule(Shift shift)
        {
            return Store.ModifyShiftOnSchedule(shift);
        }

        public string DeleteAllEmployeeShiftFromSchedule(EmployeeEntity employee)
        {
            return Store.DeleteAllEmployeeShiftFromSchedule(employee);
        }

        public string ForgotPassword(string email)
        {
            return Store.ForgotPassword(email);
        }

        public IUsernamePassword Authenticate(string username)
        {
            return Store.Authenticate(username);
        }

        public string ChangePassword(CurrentUserPassword currentUser)
        {
            return Store.ChangePassword(currentUser);
        }

        public string UpdateCompanyLogoLink(string fileLink, Guid companyId)
        {
            return Store.UpdateCompanyLogoLink(fileLink, companyId);
        }

        public string SetEmployeeShiftStatus(Shift shift)
        {
            return Store.SetEmployeeShiftStatus(shift);
        }
    }
}

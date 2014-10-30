using System;
using System.Collections.Generic;
using System.Linq;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Utils;
using Raven.Client.Document;

namespace OrdningsVaktRapport.Data.Services
{
    public class Store
    {
        private readonly DocumentStore _raventDbStore;
        private readonly BusinessRules _businessRules;
        private readonly Boolean _runRavenDbFromDocumentStore;

        public Store(bool runRavenDbFromDocumentStore = false)
        {
            _businessRules = new BusinessRules();
            _raventDbStore = (!runRavenDbFromDocumentStore) ? StoreFactory.CreateInMemoryRavenDbStore() : StoreFactory.CreateDocumentRavenDbStore();
            _runRavenDbFromDocumentStore = runRavenDbFromDocumentStore;
        }

        public string AddCompany(CompanyEntity company)
        {
            company.Username = company.EmailAddress;
            company.Password = GenerateRandomPassword(6);
            _businessRules.CompanyAddAndModifyRule(company, this, "create");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {

                    session.Store(company);
                    session.SaveChanges();
                    var message = EmailMessages.UserCreatedMessage(company.ManagerFirstname, company.ManagerLastname,
                        "Your company profile has been created, below are your user crendentials\n\n", company.Username,
                        company.Password);
                    if (_runRavenDbFromDocumentStore) SendEmail.Send(company.Username, "No Reply - Company profile", message);
                }

                return "Succeeded";
            }
            catch (Exception e)
            {

                return "UnSucceeded";
            }
        }

        public CompanyEntity GetCompanyById(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Load<CompanyEntity>(company.Id);
            }
        }

        public CompanyEntity GetCompanyByEmail(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CompanyEntity>().SingleOrDefault(s => s.EmailAddress == company.EmailAddress);

            }
        }

        public CompanyEntity GetCompanyByName(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CompanyEntity>().SingleOrDefault(s => s.Name == company.Name);
            }
        }

        public List<CompanyEntity> GetAllCompanies()
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CompanyEntity>().ToList();
            }
        }

        public string DeleteCompany(CompanyEntity company)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var loadedCompany = session.Load<CompanyEntity>(company.Id);
                    session.Delete(loadedCompany);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }

        }

        public string ModifyCompany(CompanyEntity company)
        {
            _businessRules.CompanyAddAndModifyRule(company, this, "modify");

            try
            {

                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCompany = session.Load<CompanyEntity>(company.Id);
                    currentCompany.Name = company.Name;
                    currentCompany.OrganisationNumber = company.OrganisationNumber;
                    currentCompany.VisitationAddress = company.VisitationAddress;
                    currentCompany.PostalAddress = company.PostalAddress;
                    currentCompany.EmailAddress = company.EmailAddress;
                    currentCompany.PhoneNumber = company.PhoneNumber;
                    currentCompany.MobileNumber = company.MobileNumber;
                    currentCompany.FaxNumber = company.FaxNumber;
                    currentCompany.WebsiteLink = company.WebsiteLink;
                    currentCompany.ManagerFirstname = company.ManagerFirstname;
                    currentCompany.ManagerLastname = company.ManagerLastname;
                    currentCompany.ManagerPersonalNumber = company.ManagerPersonalNumber;
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string AddEmployee(EmployeeEntity employee)
        {
            employee.Username = employee.EmailAddress;
            employee.Password = GenerateRandomPassword(6);
            _businessRules.EmployeeAddAndModifyRules(employee, this, "create");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    session.Store(employee);
                    session.SaveChanges();
                    var message = EmailMessages.UserCreatedMessage(employee.Firstname, employee.Lastname,
                        "Your employee profile has been created, bellow are your user crendentials\n\n",
                        employee.Username, employee.Password);
                    if (_runRavenDbFromDocumentStore) SendEmail.Send(employee.Username, "No Reply - Employee profile", message);
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "Unsuceeded";
            }
        }

        public EmployeeEntity GetEmployeeById(EmployeeEntity employee)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<EmployeeEntity>().SingleOrDefault(s => s.Id == employee.Id && s.CompanyId == employee.CompanyId);
            }
        }

        public EmployeeEntity GetEmployeeByEmail(EmployeeEntity employee)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<EmployeeEntity>().SingleOrDefault(s => s.EmailAddress == employee.EmailAddress && s.CompanyId == employee.CompanyId);
            }
        }

        public EmployeeEntity GetEmployeeByUsername(EmployeeEntity employee)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<EmployeeEntity>().SingleOrDefault(s => s.Username == employee.Username && s.CompanyId == employee.CompanyId);
            }
        }


        public List<EmployeeEntity> GetAllEmployee(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<EmployeeEntity>().Where(s => s.CompanyId == company.Id).ToList();
            }
        }

        public string DeleteEmployee(EmployeeEntity employee)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentEmployee = session.Query<EmployeeEntity>().SingleOrDefault(s => s.Id == employee.Id && s.CompanyId == employee.CompanyId);
                    session.Delete(currentEmployee);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }

        }

        public string ModifyEmployee(EmployeeEntity employee)
        {
            _businessRules.EmployeeAddAndModifyRules(employee, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentEmployee = session.Query<EmployeeEntity>().SingleOrDefault(s => s.Id == employee.Id && s.CompanyId == employee.CompanyId);
                    currentEmployee.JobDescription = employee.JobDescription;
                    currentEmployee.EmploymentNumber = employee.EmploymentNumber;
                    currentEmployee.PersonalNumber = employee.PersonalNumber;
                    currentEmployee.Firstname = employee.Firstname;
                    currentEmployee.Lastname = employee.Lastname;
                    currentEmployee.Address = employee.Address;
                    currentEmployee.Nationality = employee.Nationality;
                    currentEmployee.PhoneNumber = employee.PhoneNumber;
                    currentEmployee.MobileNumber = employee.MobileNumber;
                    currentEmployee.EmailAddress = employee.EmailAddress;
                    currentEmployee.BankAccount = employee.BankAccount;
                    currentEmployee.HourlyRate = employee.HourlyRate;
                    session.SaveChanges();

                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSuceeded";
            }
        }

        public string DeleteAllEmployee(CompanyEntity company)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var allEmployee = session.Query<EmployeeEntity>().Where(s => s.CompanyId == company.Id).ToList();

                    if (!allEmployee.Any()) return "Employee list is empty";

                    foreach (var employeeEntity in allEmployee)
                    {
                        session.Delete(employeeEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string AddCustomer(CustomerEntity customerEntity)
        {
            _businessRules.CustomerAddAndModifyRules(customerEntity, this, "create");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    customerEntity.Objects = new List<CustomerObject>();
                    session.Store(customerEntity);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }
        }

        public CustomerEntity GetCustomerById(CustomerEntity customerEntity)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerEntity.Id && s.CompanyId == customerEntity.CompanyId);
            }
        }

        public CustomerEntity GetCustomerByName(CustomerEntity customerEntity)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CustomerEntity>().SingleOrDefault(s => s.Name == customerEntity.Name && s.CompanyId == customerEntity.CompanyId);
            }
        }

        public List<CustomerEntity> GetAllCustomer(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CustomerEntity>().Where(s => s.CompanyId == company.Id).ToList();
            }
        }

        public string DeleteCustomer(CustomerEntity customerEntity)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var objectEntitys = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerEntity.Id && s.CompanyId == customerEntity.CompanyId);
                    session.Delete(objectEntitys);
                    session.SaveChanges();
                    return "Succeeded";

                }
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }

        }

        public string ModifyCustomer(CustomerEntity customerEntity)
        {
            _businessRules.CustomerAddAndModifyRules(customerEntity, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentObject = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerEntity.Id && s.CompanyId == customerEntity.CompanyId);
                    currentObject.OrganisationNumber = customerEntity.OrganisationNumber;
                    currentObject.Name = customerEntity.Name;
                    currentObject.VisitationAddress = customerEntity.VisitationAddress;
                    currentObject.PostalAddress = customerEntity.PostalAddress;
                    currentObject.EmailAddress = customerEntity.EmailAddress;
                    currentObject.PhoneNumber = customerEntity.PhoneNumber;
                    currentObject.MobileNumber = customerEntity.MobileNumber;
                    currentObject.FaxNumber = customerEntity.FaxNumber;
                    currentObject.WebsiteLink = customerEntity.WebsiteLink;
                    currentObject.ManagerFirstname = customerEntity.ManagerFirstname;
                    currentObject.ManagerLastname = customerEntity.ManagerLastname;
                    session.SaveChanges();

                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSuceeded";
            }

        }

        public string DeleteAllCustomer(CompanyEntity company)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var customerEnity = session.Query<CustomerEntity>().Where(s => s.CompanyId == company.Id).ToList();

                    if (!customerEnity.Any()) return "customer list is empty";

                    foreach (var objectEntity in customerEnity)
                    {
                        session.Delete(objectEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }
        }

        public string AddObjectToCustomer(CustomerObject customerObject)
        {
            _businessRules.CustomerObjectAddAndModifyRules(customerObject, this, "add");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentcustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                    customerObject.Needs = new List<Need>();
                    currentcustomer.Objects.Add(customerObject);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public CustomerObject GetCustomerObjectById(CustomerObject customerObject)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == customerObject.Id);
                return currentObject;
            }
        }

        public CustomerObject GetCustomerObjectByName(CustomerObject customerObject)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Name == customerObject.Name);
                return currentObject;
            }
        }

        public string ModifyCustomerObject(CustomerObject customerObject)
        {
            _businessRules.CustomerObjectAddAndModifyRules(customerObject, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == customerObject.Id);
                    currentObject.Name = customerObject.Name;
                    currentObject.VisitationAddress = customerObject.VisitationAddress;
                    currentObject.ResponsibleGuardFirstname = customerObject.ResponsibleGuardFirstname;
                    currentObject.ResponsibleGuardLastname = customerObject.ResponsibleGuardLastname;
                    currentObject.ResponsibleManagerFistname = customerObject.ResponsibleManagerFistname;
                    currentObject.ResponsibleManagerLastname = customerObject.ResponsibleManagerLastname;
                    currentObject.License = customerObject.License;
                    currentObject.LicenseType = customerObject.LicenseType;
                    currentObject.HourlyRate = customerObject.HourlyRate;
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }
        }

        public string DeleteCustomerObject(CustomerObject customerObject)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == customerObject.Id);
                    currentCustomer.Objects.Remove(currentObject);
                    session.SaveChanges();

                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }


        }

        public string DeleteAllCustomerObject(CustomerEntity customer)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customer.Id && s.CompanyId == customer.CompanyId);
                    if (!currentCustomer.Objects.Any()) return "customer object list is empty";
                    currentCustomer.Objects.RemoveAll(s => s.GetType() == typeof(CustomerObject));
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string AddNeedToCustomerObject(Need need)
        {
            _businessRules.CustomerObjectNeedAddAndModifyRules(need, this, "add");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                    need.StartDateTimeCombined = need.StartDateTime.ToShortDateString() + " " + need.StartDateTime.ToShortTimeString();
                    need.EndDateTimeCombined = need.EndDateTime.ToShortDateString() + " " + need.EndDateTime.ToShortTimeString();
                    currentObject.Needs.Add(need);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public Need GetNeedFromCustomerObjectById(Need need)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                var currentNeed = currentObject.Needs.FirstOrDefault(s => s.Id == need.Id);
                return currentNeed;
            }
        }

        public Need GetNeedFromCustomerObjectByStartDateTime(Need need)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                var currentNeed = currentObject.Needs.FirstOrDefault(s => s.StartDateTime == need.StartDateTime);
                return currentNeed;
            }
        }

        public Need GetNeedFromCustomerObjectByEndDateTime(Need need)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                var currentNeed = currentObject.Needs.FirstOrDefault(s => s.EndDateTime == need.EndDateTime);
                return currentNeed;
            }
        }

        public string ModifyNeedOnCustomerObject(Need need)
        {
            _businessRules.CustomerObjectNeedAddAndModifyRules(need, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                    var currentNeed = currentObject.Needs.FirstOrDefault(s => s.Id == need.Id);
                    currentNeed.NumberOfPersonalNeeded = need.NumberOfPersonalNeeded;
                    currentNeed.StartDateTime = need.StartDateTime;
                    currentNeed.EndDateTime = need.EndDateTime;
                    currentNeed.StartDateTimeCombined = need.StartDateTime.ToShortDateString() + " " + need.StartDateTime.ToShortTimeString();
                    currentNeed.EndDateTimeCombined = need.EndDateTime.ToShortDateString() + " " + need.EndDateTime.ToShortTimeString();
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string DeleteNeedFromCustomerObject(Need need)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == need.CustomerId && s.CompanyId == need.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == need.CustomerObjectId);
                    var currentNeed = currentObject.Needs.FirstOrDefault(s => s.Id == need.Id);
                    currentObject.Needs.Remove(currentNeed);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string DeleteAllNeedsFromCustomerObject(CustomerObject customerObject)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentCustomer = session.Query<CustomerEntity>().SingleOrDefault(s => s.Id == customerObject.CustomerId && s.CompanyId == customerObject.CompanyId);
                    var currentObject = currentCustomer.Objects.SingleOrDefault(s => s.Id == customerObject.Id);
                    if (!currentObject.Needs.Any()) return "customer object needs list is empty";
                    currentObject.Needs.RemoveAll(s => s.GetType() == typeof(Need));
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string AddReport(ReportEntity report)
        {
            _businessRules.ReportAddAndModifyRules(report);

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    report.ReportDate = DateTime.Now;
                    session.Store(report);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }
        }

        public ReportEntity GetReportById(ReportEntity report)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ReportEntity>().SingleOrDefault(s => s.Id == report.Id && s.CompanyId == report.CompanyId);
            }
        }

        public List<ReportEntity> GetallReports(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ReportEntity>().Where(s => s.CompanyId == company.Id).ToList();
            }
        }

        public List<ReportEntity> GetAllReportsByEmployee(EmployeeEntity employee)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ReportEntity>().Where(s => s.EmployeeId == employee.Id && s.CompanyId == employee.CompanyId).ToList();
            }
        }

        public List<ReportEntity> GetAllReportsByCustomer(CustomerEntity customer)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ReportEntity>().Where(s => s.CustomerId == customer.Id && s.CompanyId == customer.CompanyId).ToList();
            }
        }

        public string ModifyReport(ReportEntity report)
        {
            _businessRules.ReportAddAndModifyRules(report);

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var reportEntity = session.Query<ReportEntity>().SingleOrDefault(s => s.Id == report.Id && s.CompanyId == report.CompanyId);
                    reportEntity.ReportName = report.ReportName;
                    reportEntity.ReportModel = report.ReportModel;
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSuceeded";
            }
        }

        public string DeleteReport(ReportEntity report)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var reportEntity = session.Query<ReportEntity>().SingleOrDefault(s => s.Id == report.Id && s.CompanyId == report.CompanyId);
                    session.Delete(reportEntity);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }

        }

        public string DeleteAllReport(CompanyEntity company)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var reportList = session.Query<ReportEntity>().Where(s => s.CompanyId == company.Id).ToList();

                    if (!reportList.Any()) return "Report list is empty";

                    foreach (var reportEntity in reportList)
                    {
                        session.Delete(reportEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSuceeded";
            }
        }

        public string AddSchedudle(ScheduleEntity schedule)
        {
            _businessRules.ScheduleAddAndModifyRules(schedule, this, "create");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    schedule.Schedules = new List<Shift>();
                    session.Store(schedule);
                    session.SaveChanges();
                }

                return schedule.Id.ToString();
            }
            catch (Exception)
            {

                return "Unsuceeded";
            }
        }

        public ScheduleEntity GetScheduleByStartAndEndDate(ScheduleEntity schedule)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ScheduleEntity>().SingleOrDefault(
                    s => s.CustomerId == schedule.CustomerId && s.CompanyId == schedule.CompanyId && s.CustomerObjectId == schedule.CustomerObjectId && s.EndDate == schedule.EndDate && s.StartDate == schedule.StartDate);
            }
        }

        public ScheduleEntity GetScheduleById(ScheduleEntity schedule)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ScheduleEntity>().SingleOrDefault(s => s.Id == schedule.Id && s.CompanyId == schedule.CompanyId);
            }
        }

        public List<ScheduleEntity> GetCustomerSchedules(CustomerEntity customer)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ScheduleEntity>().Where(s => s.CustomerId == customer.Id && s.CompanyId == customer.CompanyId).ToList();
            }
        }

        public List<ScheduleEntity> GetCustomerObjectSchedules(CustomerObject customerObject)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ScheduleEntity>().Where(s => s.CustomerObjectId == customerObject.Id && s.CompanyId == customerObject.CompanyId).ToList();
            }
        }

        public List<ScheduleEntity> GetAllSchedules(CompanyEntity company)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<ScheduleEntity>().Where(s => s.CompanyId == company.Id).ToList();
            }
        }

        public string DeleteSchedule(ScheduleEntity schedule)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var scheduleEntity = session.Query<ScheduleEntity>().SingleOrDefault(s => s.Id == schedule.Id && s.CompanyId == schedule.CompanyId);
                    session.Delete(scheduleEntity);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string ModifySchedule(ScheduleEntity schedule)
        {
            _businessRules.ScheduleAddAndModifyRules(schedule, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var scheduleEntity = session.Query<ScheduleEntity>().SingleOrDefault(s => s.Id == schedule.Id && s.CompanyId == schedule.CompanyId);
                    scheduleEntity.StartDate = schedule.StartDate;
                    scheduleEntity.EndDate = schedule.EndDate;
                    scheduleEntity.CompanyName = schedule.CompanyName;
                    scheduleEntity.CustomerName = schedule.CustomerName;
                    scheduleEntity.CustomerObjectName = schedule.CustomerObjectName;
                    scheduleEntity.CustomerObjectAddress = schedule.CustomerObjectAddress;
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }
        }

        public string DeleteAllSchedule(CompanyEntity company)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var scheduleList = session.Query<ScheduleEntity>().Where(s => s.CompanyId == company.Id).ToList();

                    if (!scheduleList.Any()) return "Schedule list is empty";

                    foreach (var scheduleEntity in scheduleList)
                    {
                        session.Delete(scheduleEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string DeleteAllCustomerSchedule(CustomerEntity customer)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var scheduleList = session.Query<ScheduleEntity>().Where(s => s.CustomerId == customer.Id && s.CompanyId == customer.CompanyId).ToList();

                    if (!scheduleList.Any()) return "Schedule list is empty";

                    foreach (var scheduleEntity in scheduleList)
                    {
                        session.Delete(scheduleEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string DeleteAllCustomerObjectSchedule(CustomerObject customerObject)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var scheduleList = session.Query<ScheduleEntity>().Where(s => s.CustomerObjectId == customerObject.Id && s.CompanyId == customerObject.CompanyId).ToList();

                    if (!scheduleList.Any()) return "Schedule list is empty";

                    foreach (var scheduleEntity in scheduleList)
                    {
                        session.Delete(scheduleEntity);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string AddShiftToSchedule(Shift shift)
        {
            _businessRules.ShiftAddAndModifyRules(shift, this, "add");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var schedule = session.Load<ScheduleEntity>(shift.ScheduleId);
                    shift.TotalTime = shift.EndTime.Subtract(shift.StartTime).ToString();
                    schedule.Schedules.Add(shift);
                    session.SaveChanges();
                    var message = EmailMessages.ShifNotificationMessage(shift.Firstname, shift.Lastname,
                        "Hi you have been assigned to a schedule, the details are as follows\n\n",
                        schedule.CustomerObjectName, schedule.CustomerObjectAddress, shift.StartTime, shift.EndTime);
                    if (_runRavenDbFromDocumentStore) SendEmail.Send(shift.EmailAddress, "No Reply - Employee Schedule", message);
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public Shift GetShiftById(Shift shift)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var schedule = session.Load<ScheduleEntity>(shift.ScheduleId);
                return schedule.Schedules.SingleOrDefault(s => s.Id == shift.Id);
            }
        }

        public List<Shift> GetAllEmployeeShifts(EmployeeEntity employee)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var schedules = session.Query<ScheduleEntity>().Where(s => s.CompanyId == employee.CompanyId);
                var shiftList = new List<Shift>();
                foreach (var scheduleEntity in schedules)
                {
                    foreach (var shift in scheduleEntity.Schedules.Where(shift => shift.EmployeeId == employee.Id))
                    {
                        shiftList.Add(shift);
                    }

                }
                return shiftList;
            }
        }

        public Shift GetShiftByEmployee(Shift shifts)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var schedules = session.Query<ScheduleEntity>().Where(s => s.CompanyId == shifts.CompanyId);
                var shiftList = new List<Shift>();
                foreach (var scheduleEntity in schedules)
                {
                    foreach (var shift in scheduleEntity.Schedules.Where(shift => shift.EmployeeId == shifts.EmployeeId))
                    {
                        shiftList.Add(shift);
                    }

                }
                return shiftList.SingleOrDefault(s => s.StartTime == shifts.StartTime && s.EndTime == shifts.EndTime);
            }
        }

        public string DeleteAllEmployeeShiftFromSchedule(EmployeeEntity employee)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var schedules = session.Query<ScheduleEntity>().Where(s => s.CompanyId == employee.CompanyId);
                    if (!schedules.Any()) return "Schedule list is empty";
                    foreach (var scheduleEntity in schedules)
                    {
                        scheduleEntity.Schedules.RemoveAll(shift => shift.EmployeeId == employee.Id);
                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }


        }

        public string DeleteShiftFromSchedule(Shift shift)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var schedule = session.Load<ScheduleEntity>(shift.ScheduleId);
                    var currentShift = schedule.Schedules.SingleOrDefault(s => s.Id == shift.Id);
                    schedule.Schedules.Remove(currentShift);
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }

        }

        public string DeleteAllShiftFromSchedule(ScheduleEntity schedule)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentschedule = session.Load<ScheduleEntity>(schedule.Id);
                    if (!currentschedule.Schedules.Any()) return "This Schedule don't have any shift";
                    currentschedule.Schedules.RemoveAll(s => s.GetType() == typeof(Shift));
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string ModifyShiftOnSchedule(Shift shift)
        {
            _businessRules.ShiftAddAndModifyRules(shift, this, "modify");

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var schedule = session.Load<ScheduleEntity>(shift.ScheduleId);
                    if (shift.Removed)
                    {
                        var currentShift = schedule.Schedules.SingleOrDefault(s => s.Id == shift.Id);
                        schedule.Schedules.Remove(currentShift);
                    }
                    else
                    {
                        var currentShift = schedule.Schedules.SingleOrDefault(s => s.Id == shift.Id);
                        if (currentShift != null)
                        {
                            currentShift.Firstname = shift.Firstname;
                            currentShift.Lastname = shift.Lastname;
                            currentShift.StartTime = shift.StartTime;
                            currentShift.EndTime = shift.EndTime;
                            currentShift.Status = shift.Status;
                            currentShift.EmailAddress = shift.EmailAddress;
                            currentShift.JobDescription = shift.JobDescription;
                            currentShift.PersonalNumber = shift.PersonalNumber;
                            currentShift.TotalTime = shift.EndTime.Subtract(shift.StartTime).ToString();
                        }
                        else
                        {
                            shift.Id = Guid.NewGuid();
                            schedule.Schedules.Add(shift);
                        }

                    }

                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string SetEmployeeShiftStatus(Shift shift)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var schedule = session.Load<ScheduleEntity>(shift.ScheduleId);
                    var employeeShift = schedule.Schedules.SingleOrDefault(e => e.Id == shift.Id);
                    employeeShift.Status =  shift.Status;
                    var currentCompany = session.Load<CompanyEntity>(shift.CompanyId);
                    var message = EmailMessages.ShiftStatusChangedNotification(currentCompany.ManagerFirstname,
                        currentCompany.ManagerLastname, shift.Firstname, shift.Lastname, shift.CustomerObjectAddress,
                        shift.StartTime, shift.EndTime, shift.CustomerObjectName, shift.Status,
                        "Schedule status reply\n\n");

                    session.SaveChanges();
                    if (_runRavenDbFromDocumentStore) SendEmail.Send(currentCompany.EmailAddress, "No Reply - Employee Schedule", message);
                }

                return "Succeeded";
            }
            catch (Exception)
            {

                return "UnSucceeded";
            }

        }

        public string ForgotPassword(string email)
        {
            _businessRules.ForgotPasswordRules(email);

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var currentEmployee = session.Query<EmployeeEntity>().SingleOrDefault(s => s.EmailAddress == email);
                    var currentCompany = session.Query<CompanyEntity>().SingleOrDefault(s => s.EmailAddress == email);

                    if (currentEmployee != null)
                    {
                        currentEmployee.Password = GenerateRandomPassword(6);
                        session.SaveChanges();
                        var message = EmailMessages.UserCreatedMessage(currentEmployee.Firstname,
                            currentEmployee.Lastname,
                            "Your company profile has been changed, below are your user crendentials\n\n",
                            currentEmployee.Username, currentEmployee.Password);

                        if (_runRavenDbFromDocumentStore) SendEmail.Send(currentEmployee.EmailAddress, "No Reply - Employee profile", message);
                        return "Succeeded";
                    }

                    if (currentCompany != null)
                    {
                        currentCompany.Password = GenerateRandomPassword(6);
                        session.SaveChanges();

                        var message = EmailMessages.UserCreatedMessage(currentCompany.ManagerFirstname, currentCompany.ManagerLastname,
                           "Your company profile has been changed, below are your user crendentials\n\n",
                           currentCompany.Username, currentCompany.Password);

                        if (_runRavenDbFromDocumentStore) SendEmail.Send(currentCompany.EmailAddress, "No Reply - Company profile", message);
                        return "Succeeded";
                    }

                }

                return "UnSucceeded";
            }
            catch (Exception)
            {
                return "UnSucceeded";
            }
        }

        public string ChangePassword(CurrentUserPassword currenUser)
        {
            _businessRules.ChangePasswordRules(currenUser);

            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    if (currenUser.UserType == "Company")
                    {
                        var company = session.Query<CompanyEntity>().SingleOrDefault(u => u.Username == currenUser.Username && u.Password == currenUser.OldPassword);
                        company.Password = currenUser.NewPassword;
                        session.SaveChanges();

                        var message = EmailMessages.UserCreatedMessage(company.ManagerFirstname, company.ManagerLastname,
                            "Your company profile has been changed, below are your user crendentials\n\n",
                            currenUser.Username, currenUser.NewPassword);

                        if (_runRavenDbFromDocumentStore) SendEmail.Send(company.EmailAddress, "No Reply - Company profile", message);
                        return "Succeeded";
                    }

                    if (currenUser.UserType == "Employee")
                    {
                        var employee = session.Query<EmployeeEntity>().SingleOrDefault(u => u.Username == currenUser.Username && u.Password == currenUser.OldPassword);
                        employee.Password = currenUser.NewPassword;
                        session.SaveChanges();

                        var message = EmailMessages.UserCreatedMessage(employee.Firstname, employee.Lastname,
                            "Your company profile has been changed, below are your user crendentials\n\n",
                            currenUser.Username, currenUser.NewPassword);

                        if (_runRavenDbFromDocumentStore) SendEmail.Send(employee.EmailAddress, "No Reply - Employee profile", message);
                        return "Succeeded";
                    }

                }

                return "UnSucceeded";
            }
            catch (Exception e)
            {

                return "UnSucceeded";
            }

        }

        public string GenerateRandomPassword(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var password = string.Empty;
            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);
                //Don't Allow Repetation of Characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }

            return password;

        }

        public EmployeeEntity CheckIfEmployeeEmailExist(string email)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<EmployeeEntity>().SingleOrDefault(s => s.EmailAddress == email);
            }
        }

        public CompanyEntity CheckIfCompanyEmailExist(string email)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                return session.Query<CompanyEntity>().SingleOrDefault(s => s.EmailAddress == email);
            }
        }

        public IUsernamePassword Authenticate(string username)
        {
            using (var session = _raventDbStore.OpenSession())
            {
                var company = session.Query<CompanyEntity>().SingleOrDefault(s => s.Username == username);
                var employee = session.Query<EmployeeEntity>().SingleOrDefault(s => s.Username == username);

                if (company != null)
                {
                    return company;
                }

                if (employee != null)
                {
                    return employee;
                }

                return null;
            }
        }

        public string UpdateCompanyLogoLink(string fileLink, Guid companyId)
        {
            try
            {
                using (var session = _raventDbStore.OpenSession())
                {
                    var company = session.Load<CompanyEntity>(companyId);
                    company.LogoLink = fileLink;
                    session.SaveChanges();
                }

                return "Succeeded";
            }
            catch (Exception e)
            {
                return "UnSucceeded";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.ScheduleEntityTests
{
    class when_deleting_all_schedules_from_a_company
    {
        protected static Store Store;
        protected static IRepository Repository;
        protected static CompanyEntity Company;
        protected static CustomerEntity Customer;
        protected static EmployeeEntity Employee;
        protected static CustomerObject CustomerObject;
        protected static ScheduleEntity Schedule;
        protected static ScheduleEntity ReturnedSchedule;
        protected static Guid CompanyId;
        protected static Guid CustomerId;
        protected static Guid EmployeeId;
        protected static Guid CustomerObjectId;
        protected static Guid ScheduleId;
        protected static string Responses;
        protected static List<ScheduleEntity> AllCompanySchedules;
        protected static List<ScheduleEntity> AllCompanySchedulesAfterDelete;

        private Establish context = () =>
        {
            Store = new Store();
            Repository = new Repository(Store);
            CompanyId = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            EmployeeId = Guid.NewGuid();
            CustomerObjectId = Guid.NewGuid();

            Company = new CompanyEntity { Id = CompanyId, Name = "New Company", EmailAddress = "newcompany@hotmail.com" };
            Responses = Repository.AddCompany(Company);
            Customer = new CustomerEntity { Id = CustomerId, CompanyId = CompanyId, Name = "new customer" };
            Responses = Repository.AddCustomer(Customer);
            Employee = new EmployeeEntity
            {
                Id = EmployeeId,
                CompanyId = CompanyId,
                Firstname = "Employee Firstname",
                Lastname = "Employee lastname",
                EmailAddress = "Employee@hotmail.com"
            };
            Responses = Repository.AddEmployee(Employee);
            CustomerObject = new CustomerObject
            {
                Id = CustomerObjectId,
                CustomerId = CustomerId,
                CompanyId = CompanyId,
                Name = "New Customer Object"
            };
            Responses = Repository.AddObjectToCustomer(CustomerObject);
            Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            var schedule = new ScheduleEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = DateTime.Now.AddHours(1)
            };

            var schedule2 = new ScheduleEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = DateTime.Now.AddHours(1)
            };

            var schedule3 = new ScheduleEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = DateTime.Now.AddHours(1)
            };

            Responses = Repository.AddSchedule(schedule);
            Responses = Repository.AddSchedule(schedule2);
            Responses = Repository.AddSchedule(schedule3);
            AllCompanySchedules = Repository.GetAllSchedules(Company);
            Responses = Repository.DeleteAllSchedule(Company);
            AllCompanySchedulesAfterDelete = Repository.GetAllSchedules(Company);
        };

        private It should_delete_all_a_schedules_from_a_company = () =>
        {
            Responses.ShouldEqual("Succeeded");
            AllCompanySchedules.Count.ShouldEqual(3);
            AllCompanySchedulesAfterDelete.Count.ShouldEqual(0);
        };

    }
}

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
    class when_modifying_a_schedule_from_a_company
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
        protected static DateTime DateBeforeModify;
        protected static DateTime DateAfterModify;
        protected static DateTime ScheduleDateBefore;
        protected static DateTime ScheduleDateAfter;

        private Establish context = () =>
        {
            Store = new Store();
            Repository = new Repository(Store);
            CompanyId = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            EmployeeId = Guid.NewGuid();
            CustomerObjectId = Guid.NewGuid();
            DateBeforeModify = DateTime.Now.AddHours(1);
            DateAfterModify = DateTime.Now.AddHours(2);

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

            Schedule = new ScheduleEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = DateBeforeModify
            };
            Responses = Repository.AddSchedule(Schedule);

           Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            var returnedSchedule = Repository.GetScheduleById(Schedule);
            ScheduleDateBefore = returnedSchedule.StartDate;
            returnedSchedule.StartDate = DateAfterModify;
            Responses = Repository.ModifySchedule(returnedSchedule);
            var returnedScheduleAfterModify = Repository.GetScheduleById(returnedSchedule);
            ScheduleDateAfter = returnedScheduleAfterModify.StartDate;
        };

        private It should_modify_a_schedules_from_a_company = () =>
        {
            ScheduleDateBefore.ShouldEqual(DateBeforeModify);
            ScheduleDateAfter.ShouldEqual(DateAfterModify);
        };
    }
}

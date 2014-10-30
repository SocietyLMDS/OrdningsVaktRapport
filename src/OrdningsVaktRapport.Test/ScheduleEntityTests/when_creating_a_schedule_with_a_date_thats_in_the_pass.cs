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
    class when_creating_a_schedule_with_a_date_thats_in_the_pass
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
        protected static string Reponses;
        protected static Exception Exception;

        private Establish context = () =>
        {
            Store = new Store();
            Repository = new Repository(Store);
            CompanyId = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            EmployeeId = Guid.NewGuid();
            CustomerObjectId = Guid.NewGuid();

            Company = new CompanyEntity { Id = CompanyId, Name = "New Company", EmailAddress = "newcompany@hotmail.com" };
            Reponses = Repository.AddCompany(Company);
            Customer = new CustomerEntity { Id = CustomerId, CompanyId = CompanyId, Name = "new customer" };
            Reponses = Repository.AddCustomer(Customer);
            Employee = new EmployeeEntity
            {
                Id = EmployeeId,
                CompanyId = CompanyId,
                Firstname = "Employee Firstname",
                Lastname = "Employee lastname",
                EmailAddress = "Employee@hotmail.com"
            };
            Reponses = Repository.AddEmployee(Employee);
            CustomerObject = new CustomerObject
            {
                Id = CustomerObjectId,
                CustomerId = CustomerId,
                CompanyId = CompanyId,
                Name = "New Customer Object"
            };
            Reponses = Repository.AddObjectToCustomer(CustomerObject);
            Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            ScheduleId = Guid.NewGuid();
            Schedule = new ScheduleEntity
            {
                Id = ScheduleId,
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = DateTime.Now.AddMinutes(-20)
            };

            Exception = Catch.Exception(() => Repository.AddSchedule(Schedule));
        };

        private It should_throw_an_exception = () =>
        {
            Exception.Message.ShouldEqual("You cannot create a new schedule with a date that's in the pass");
        };

    }
}

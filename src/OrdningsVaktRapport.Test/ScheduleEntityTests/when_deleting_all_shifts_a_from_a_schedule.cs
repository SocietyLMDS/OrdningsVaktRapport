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
    class when_deleting_all_shifts_a_from_a_schedule
    {
        protected static Store Store;
        protected static IRepository Repository;
        protected static CompanyEntity Company;
        protected static CustomerEntity Customer;
        protected static EmployeeEntity Employee;
        protected static CustomerObject CustomerObject;
        protected static ScheduleEntity Schedule;
        protected static ScheduleEntity ReturnedSchedule;
        protected static ScheduleEntity ReturnedScheduleAfterDelete;
        protected static Shift Shift;
        protected static Shift ReturnedShift;
        protected static Guid CompanyId;
        protected static Guid CustomerId;
        protected static Guid EmployeeId;
        protected static Guid CustomerObjectId;
        protected static Guid ScheduleId;
        protected static Guid ShiftId;
        protected static string Responses;
        protected static DateTime ScheduleDate;
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

            ScheduleId = Guid.NewGuid();
            ScheduleDate = DateTime.Now.AddMinutes(10);
            Schedule = new ScheduleEntity
            {
                Id = ScheduleId,
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                CustomerObjectId = CustomerObjectId,
                StartDate = ScheduleDate,

            };
            Responses = Repository.AddSchedule(Schedule);

            Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            for (var i = 0; i < 3; i++)
            {
                var shiftId = Guid.NewGuid();
                var shift = new Shift
                {
                    Id = shiftId,
                    EmployeeId = EmployeeId,
                    ScheduleId = ScheduleId,
                    Firstname = Employee.Firstname + " " + i,
                    Lastname = Employee.Lastname + " " + i,
                    StartTime = DateTime.Now.AddMinutes(20),
                    EndTime = DateTime.Now.AddMinutes(60),
                    Status = "Assigned"
                };

                Repository.AddShiftToSchedule(shift);
            }

            ReturnedSchedule = Repository.GetScheduleById(Schedule);
            Responses = Repository.DeleteAllShiftFromSchedule(Schedule);
            ReturnedScheduleAfterDelete = Repository.GetScheduleById(Schedule);

        };

        private It should_delete_all_shifts_from_a_schedule = () =>
        {
            ReturnedSchedule.Schedules.Count.ShouldEqual(3);
            Responses.ShouldEqual("Succeeded");
            ReturnedScheduleAfterDelete.Schedules.Count.ShouldEqual(0);
        };
    }
}

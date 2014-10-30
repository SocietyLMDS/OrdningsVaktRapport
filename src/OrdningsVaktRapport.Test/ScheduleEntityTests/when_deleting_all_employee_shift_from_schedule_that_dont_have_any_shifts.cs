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
    class when_deleting_all_employee_shift_from_a_company_that_dont_have_any_schedules
    {
        protected static Store Store;
        protected static IRepository Repository;
        protected static CompanyEntity Company;
        protected static CustomerEntity Customer;
        protected static EmployeeEntity Employee;
        protected static EmployeeEntity Employee2;
        protected static EmployeeEntity Employee3;
        protected static CustomerObject CustomerObject;
        protected static ScheduleEntity Schedule;
        protected static ScheduleEntity ReturnedSchedule;
        protected static Shift Shift;
        protected static Shift ReturnedShift;
        protected static Guid CompanyId;
        protected static Guid CustomerId;
        protected static Guid EmployeeId;
        protected static Guid EmployeeId2;
        protected static Guid EmployeeId3;
        protected static Guid CustomerObjectId;
        protected static Guid ScheduleId;
        protected static Guid ShiftId;
        protected static string Responses;
        protected static DateTime ScheduleDate;
        protected static Exception Exception;
        protected static List<Shift> EmployeeShifts;
        protected static List<Shift> Employee2Shifts;
        protected static List<Shift> Employee2ShiftsAfterDelete;
        protected static List<Shift> Employee3Shifts;


        private Establish context = () =>
        {
            Store = new Store();
            Repository = new Repository(Store);
            CompanyId = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            EmployeeId = Guid.NewGuid();
            EmployeeId2 = Guid.NewGuid();
            EmployeeId3 = Guid.NewGuid();
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

            Employee2 = new EmployeeEntity
            {
                Id = EmployeeId2,
                CompanyId = CompanyId,
                Firstname = "Ladji",
                Lastname = "Diakite",
                EmailAddress = "ladji@hotmail.com"
            };

            Responses = Repository.AddEmployee(Employee2);

            Employee3 = new EmployeeEntity
            {
                Id = EmployeeId3,
                CompanyId = CompanyId,
                Firstname = "Seydou",
                Lastname = "Diakite",
                EmailAddress = "Seydou@hotmail.com"
            };

            Responses = Repository.AddEmployee(Employee3);

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

                Responses = Repository.DeleteAllEmployeeShiftFromSchedule(Employee2);

            };

        private It should_return_response_schedule_list_is_empty_ = () =>
            {
                Responses.ShouldEqual("Schedule list is empty");
            };
    }
}

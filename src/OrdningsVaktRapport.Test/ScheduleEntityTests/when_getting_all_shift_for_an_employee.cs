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
    class when_getting_all_shift_for_an_employee
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
        protected static string Reponses;
        protected static DateTime ScheduleDate;
        protected static Exception Exception;
        protected static List<Shift> EmployeeShifts;
        protected static List<Shift> Employee2Shifts;
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

            Employee2 = new EmployeeEntity
            {
                Id = EmployeeId2,
                CompanyId = CompanyId,
                Firstname = "Ladji",
                Lastname = "Diakite",
                EmailAddress = "ladji@hotmail.com"
            };

            Reponses = Repository.AddEmployee(Employee2);

            Employee3 = new EmployeeEntity
            {
                Id = EmployeeId3,
                CompanyId = CompanyId,
                Firstname = "Seydou",
                Lastname = "Diakite",
                EmailAddress = "Seydou@hotmail.com"
            };

            Reponses = Repository.AddEmployee(Employee3);

            CustomerObject = new CustomerObject
            {
                Id = CustomerObjectId,
                CustomerId = CustomerId,
                CompanyId = CompanyId,
                Name = "New Customer Object"
            };
            Reponses = Repository.AddObjectToCustomer(CustomerObject);

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
            Reponses = Repository.AddSchedule(Schedule);

            Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            for (var i = 0; i < 3; i++)
            {
                var shiftId = Guid.NewGuid();
                var shift = new Shift
                {
                    Id = ShiftId,
                    EmployeeId = EmployeeId,
                    ScheduleId = ScheduleId,
                    Firstname = Employee.Firstname,
                    Lastname = Employee.Lastname,
                    StartTime = DateTime.Now.AddMinutes(20),
                    EndTime = DateTime.Now.AddMinutes(60),
                    Status = "Assigned"
                };

                Repository.AddShiftToSchedule(shift);
            }
            
            for (var i = 0; i < 3; i++)
            {
                var shiftId2 = Guid.NewGuid();
                var shift = new Shift
                {
                    Id = shiftId2,
                    EmployeeId = EmployeeId2,
                    ScheduleId = ScheduleId,
                    Firstname = Employee2.Firstname,
                    Lastname = Employee2.Lastname,
                    StartTime = DateTime.Now.AddMinutes(20),
                    EndTime = DateTime.Now.AddMinutes(60),
                    Status = "Assigned"
                };

                Repository.AddShiftToSchedule(shift);
            }

            for (var i = 0; i < 3; i++)
            {
                var shiftId3 = Guid.NewGuid();
                var shift = new Shift
                {
                    Id = shiftId3,
                    EmployeeId = EmployeeId3,
                    ScheduleId = ScheduleId,
                    Firstname = Employee3.Firstname,
                    Lastname = Employee3.Lastname,
                    StartTime = DateTime.Now.AddMinutes(20),
                    EndTime = DateTime.Now.AddMinutes(60),
                    Status = "Assigned"
                };

                Repository.AddShiftToSchedule(shift);
            }

            EmployeeShifts = Repository.GetAllEmployeeShifts(Employee);
            Employee2Shifts = Repository.GetAllEmployeeShifts(Employee2);
            Employee3Shifts = Repository.GetAllEmployeeShifts(Employee3);

        };

        private It should_return_all_shifts_for_an_employee = () =>
            {
                EmployeeShifts.Count.ShouldEqual(3);
                Employee2Shifts.Count.ShouldEqual(3);
                Employee3Shifts.Count.ShouldEqual(3);
            };
    }
}

﻿using System;
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
    class when_modifying_a_shift_on_schedule_with_all_the_details_needed
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
        protected static Shift SavedShift;
        protected static Shift ShiftBeforeModified;
        protected static Shift ModifiedShift;
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
        protected static List<Shift> Shifts; 

        private Establish context = () =>
        {
            Store = new Store();
            Repository = new Repository(Store);
            CompanyId = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            EmployeeId = Guid.NewGuid();
            CustomerObjectId = Guid.NewGuid();
            Shifts = new List<Shift>();

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

            for (var i = 0; i < 3; i++)
            {
                var shiftId = Guid.NewGuid();
                var shift = new Shift
                {
                    Id = shiftId,
                    EmployeeId = EmployeeId,
                    ScheduleId = ScheduleId,
                    Firstname = Employee.Firstname,
                    Lastname = Employee.Lastname,
                    StartTime = DateTime.Now.AddMinutes(20),
                    EndTime = DateTime.Now.AddMinutes(60),
                    Status = "Assigned"
                };

                Shifts.Add(shift);
                Repository.AddShiftToSchedule(shift);
            }

            Thread.Sleep(2000);
        };

        private Because of = () =>
            {
                ShiftBeforeModified = Repository.GetShiftById(Shifts[0]);
                SavedShift = Repository.GetShiftById(Shifts[0]);
                SavedShift.Firstname = "Ladji";
                SavedShift.Lastname = "Kapis";
                Responses = Repository.ModifyShiftOnSchedule(SavedShift);
                ModifiedShift = Repository.GetShiftById(Shifts[0]);
            };

        private It should_modify_shift = () =>
            {
                ShiftBeforeModified.Firstname.ShouldEqual("Employee Firstname");
                ShiftBeforeModified.Lastname.ShouldEqual("Employee lastname");
                ModifiedShift.Firstname.ShouldEqual("Ladji");
                ModifiedShift.Lastname.ShouldEqual("Kapis");
            };
    }
}

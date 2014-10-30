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

namespace OrdningsVaktRapport.Test.EmployeeEntityTests
{
    class when_creating_an_employee_with_an_email_address_that_already_exist
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly EmployeeEntity Employee = new EmployeeEntity();
        private static readonly EmployeeEntity Employee2 = new EmployeeEntity();
        private static CompanyEntity _companySaved = new CompanyEntity();
        private static string _companyResponse;
        private static string _employeeResponse;
        private static string _employeeResponce2;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid EmployeeId = Guid.NewGuid();
        private static readonly Guid Employee2Id = Guid.NewGuid();
        private static Exception exception;

        private Establish Context = () =>
        {
            Company.Id = CompanyId;
            Company.Name = "Svea Vaktbolag";
            Company.VisitationAddress = new Address { Street = "Sveavägen 14", Postcode = "15161 Stockholm" };
            Company.ManagerFirstname = "Ladji";
            Company.ManagerLastname = "diakite";
            Company.ManagerPersonalNumber = "197708090894";
            Company.EmailAddress = "newemailaddress@hotmail.com";
            Company.AuthorisationLink = "http://www.link.com";
            _companyResponse = Repository.AddCompany(Company);
            Thread.Sleep(2000);

            Employee.Id = EmployeeId;
            Employee.CompanyId = CompanyId;
            Employee.Firstname = "John";
            Employee.Lastname = "Doe";
            Employee.PersonalNumber = "198509060782";
            Employee.Address = new Address { Street = "Vissgatan 65", Postcode = "14568 Stockholm" };
            Employee.EmailAddress = "employeeemail@hotmail.com";
            Employee.MobileNumber = "0768545690";
            Employee.BankAccount = "0768545690";
            Employee.HourlyRate = 200;
            _employeeResponse = Repository.AddEmployee(Employee);
            Thread.Sleep(2000);
        };

        private Because of = () =>
        {
            Employee2.Id = Employee2Id;
            Employee2.CompanyId = CompanyId;
            Employee2.Firstname = "John";
            Employee2.Lastname = "Doe";
            Employee2.PersonalNumber = "198509060782";
            Employee2.Address = new Address { Street = "Vissgatan 65", Postcode = "14568 Stockholm" };
            Employee2.EmailAddress = "employeeemail@hotmail.com";
            Employee2.MobileNumber = "0768545690";
            Employee2.BankAccount = "0768545690";
            Employee2.HourlyRate = 200;
            exception = Catch.Exception(() => Repository.AddEmployee(Employee2));
        };

        private It an_exception_should_be_thrown = () =>
            {
                exception.Message.ShouldEqual("An employee with that email address already exist");
            };
    }
}

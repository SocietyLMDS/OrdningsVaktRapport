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
    class when_modifying_an_employee_without_an_email_address
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly EmployeeEntity Employee = new EmployeeEntity();
        private static CompanyEntity _companySaved = new CompanyEntity();
        private static EmployeeEntity _employeeSaved = new EmployeeEntity();
        private static EmployeeEntity _employeeModified;
        private static string _companyResponse;
        private static string _employeeResponse;
        private static string _employeeModifiedResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid EmployeeId = Guid.NewGuid();
        private static Exception _exception;

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
            _companySaved = Repository.GetCompanyById(Company);
            _employeeSaved = Repository.GetEmployeeById(Employee);
            _employeeSaved.Firstname = "Jessica";
            _employeeSaved.Lastname = "Wilkinsson";
            _employeeSaved.EmailAddress = "";
            _employeeSaved.PersonalNumber = "198209203970";
            _exception = Catch.Exception(() => Repository.ModifyEmployee(_employeeSaved));
            Thread.Sleep(2000);
        };

        private It should_create_a_new_employee = () =>
        {
            _employeeSaved.Id.ShouldEqual(EmployeeId);
            _employeeSaved.CompanyId.ShouldEqual(CompanyId);
            _exception.Message.ShouldEqual("You cannot modify a new employee without an email address");
        };
    }
}

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
    class when_creating_an_employee_with_an_invalid_email_address
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly EmployeeEntity Employee = new EmployeeEntity();
        private static Exception _exception;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid EmployeeId = Guid.NewGuid();
        private static string _companyResponse;
        private static string _employeeResponse;

        private Establish context = () =>
        {
            Company.Id = CompanyId;
            Company.Name = "Stockholms Vakt Bolag";
            Company.ManagerFirstname = "Manager";
            Company.ManagerLastname = "Olsson";
            Company.ManagerPersonalNumber = "19788870905";
            Company.VisitationAddress = new Address { Street = "stockholmsgatan", Postcode = "36389 stockholm" };
            Company.EmailAddress = "ladji.diakite@gmail.com";
            Company.AuthorisationLink = "http://www.link.se";
            _companyResponse = Repository.AddCompany(Company);
            Thread.Sleep(2000);

        };

        private Because of = () =>
        {

            Employee.Id = EmployeeId;
            Employee.CompanyId = CompanyId;
            Employee.Firstname = "John";
            Employee.Lastname = "Doe";
            Employee.PersonalNumber = "198509060782";
            Employee.Address = new Address { Street = "Vissgatan 65", Postcode = "14568 Stockholm" };
            Employee.EmailAddress = "ladji.slsl@asascom";
            Employee.MobileNumber = "0768545690";
            Employee.BankAccount = "0768545690";
            Employee.HourlyRate = 200;
            _exception = Catch.Exception(() => Repository.AddEmployee(Employee));
            Thread.Sleep(2000);
        };

        private It an_exception_should_be_thrown = () =>
        {
            _companyResponse.ShouldEqual("Succeeded");
            _exception.Message.ShouldEqual("The email address is not a valid format");
        };
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;
using Machine.Specifications;

namespace OrdningsVaktRapport.Test.EmployeeEntityTests
{
    class when_retrieving_all_employees_for_a_company_that_have_employees
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly EmployeeEntity Employee1 = new EmployeeEntity();
        private static readonly EmployeeEntity Employee2 = new EmployeeEntity();
        private static string _companyResponse;
        private static string _employeeResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid Employee1Id = Guid.NewGuid();
        private static readonly Guid Employee2Id = Guid.NewGuid();
        private static List<EmployeeEntity> _employeeList; 

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

                Employee1.Id = Employee1Id;
                Employee1.CompanyId = CompanyId;
                Employee1.Firstname = "John";
                Employee1.Lastname = "Doe";
                Employee1.PersonalNumber = "198509060782";
                Employee1.Address = new Address { Street = "Vissgatan 65", Postcode = "14568 Stockholm" };
                Employee1.EmailAddress = "employeeemail@hotmail.com";
                Employee1.MobileNumber = "0768545690";
                Employee1.BankAccount = "0768545690";
                Employee1.HourlyRate = 200;
                _employeeResponse = Repository.AddEmployee(Employee1);
                Thread.Sleep(2000);

                Employee2.Id = Employee2Id;
                Employee2.CompanyId = CompanyId;
                Employee2.Firstname = "Katarina";
                Employee2.Lastname = "Ramstedt";
                Employee2.PersonalNumber = "198509060852";
                Employee2.Address = new Address { Street = "Stockholms 65", Postcode = "14564 Stockholm" };
                Employee2.EmailAddress = "Katarinaemployeeemail@hotmail.com";
                Employee2.MobileNumber = "07685456904";
                Employee2.BankAccount = "07685456904";
                Employee2.HourlyRate = 200;
                _employeeResponse = Repository.AddEmployee(Employee2);
                Thread.Sleep(2000);
            };

        private Because Of = () =>
            {
                
                _employeeList = Repository.GetAllEmployee(Company);
            };

        private It should_return_all_employees_that_belongs_to_a_company = () =>
            {
                _employeeList.Count.ShouldBeGreaterThan(0);
                var employee1 = _employeeList.ElementAt(0);
                var employee2 = _employeeList.ElementAt(1);
                employee1.Firstname.ShouldEqual("John");
                employee2.Firstname.ShouldEqual("Katarina");
            };
    }
}

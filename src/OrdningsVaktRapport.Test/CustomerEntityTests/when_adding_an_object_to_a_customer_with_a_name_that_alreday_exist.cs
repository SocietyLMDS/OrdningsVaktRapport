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

namespace OrdningsVaktRapport.Test.CustomerEntityTests
{
    class when_adding_an_object_to_a_customer_with_a_name_that_alreday_exist
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static CustomerEntity _customerReturned = new CustomerEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static string _companyResponse;
        private static string _customerObjecResponse;
        private static Exception _exception;

        private Establish Context = () =>
        {
            Company.Id = CompanyId;
            Company.Name = "Sverige VaktBolag";
            Company.VisitationAddress = new Address { Street = "Visättavägen 20", Postcode = "14161 Huddinge" };
            Company.ManagerFirstname = "Ladji";
            Company.ManagerLastname = "Diakite";
            Company.ManagerPersonalNumber = "7708090894";
            Company.EmailAddress = "ladji@gmail.com";
            Company.AuthorisationLink = "http://www.link.se";
            _companyResponse = Repository.AddCompany(Company);
            Thread.Sleep(2000);


            Customer.Id = CustomerId;
            Customer.CompanyId = CompanyId;
            Customer.Name = "Marie Lauvaut";
            Customer.VisitationAddress = new Address { Street = "Hornsgatan 60", Postcode = "14180 Stockholm" };
            Customer.ManagerFirstname = "Manager Firstname";
            Customer.ManagerLastname = "Manager Lastname";
            _customerObjecResponse = Repository.AddCustomer(Customer);
            Thread.Sleep(2000);
        };

        private Because Of = () =>
        {
            var customerObject = new CustomerObject
            {
                Id = Guid.NewGuid(),
                Name = "sture",
                ResponsibleGuardFirstname = "ResponsibleGuardFirstname",
                ResponsibleGuardLastname = "ResponsibleGuardLastname",
                ResponsibleManagerFistname = "ResponsibleManagerFistname",
                ResponsibleManagerLastname = "ResponsibleManagerLastname",
                HourlyRate = 200,
                License = true,
                LicenseType = "LicenseType",
                CustomerId = CustomerId,
                CompanyId = CompanyId
            };

            _customerObjecResponse = Repository.AddObjectToCustomer(customerObject);

            var customerObject2 = new CustomerObject
            {
                Id = Guid.NewGuid(),
                Name = "sture",
                ResponsibleGuardFirstname = "ResponsibleGuardFirstname",
                ResponsibleGuardLastname = "ResponsibleGuardLastname",
                ResponsibleManagerFistname = "ResponsibleManagerFistname",
                ResponsibleManagerLastname = "ResponsibleManagerLastname",
                HourlyRate = 200,
                License = true,
                LicenseType = "LicenseType",
                CustomerId = CustomerId,
                CompanyId = CompanyId
            };

            _exception = Catch.Exception(()=>Repository.AddObjectToCustomer(customerObject2));
        };

        private It should_throw_an_exception = () =>
        {
            _exception.Message.ShouldEqual("An object with that name already exist");
        };
    }
}

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
    class when_creating_a_customer_with_all_the_details
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static CompanyEntity _companySaved;
        private static CustomerEntity _customerSaved;
        private static string _companyResponse;
        private static string _customerResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();

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
                Customer.Name = "Marie Lauvant";
                Customer.VisitationAddress = new Address { Street = "Hornsgatan 60", Postcode = "14180 Stockholm" };
                Customer.ManagerFirstname = "Manager Firstname";
                Customer.ManagerLastname = "Manager Lastname";
                _customerResponse = Repository.AddCustomer(Customer);
                Thread.Sleep(2000);

            };

        private Because Of = () =>
            {
                _companySaved = Repository.GetCompanyById(Company);
                _customerSaved = Repository.GetCustomerById(Customer);
            };

        private It should_create_a_object_for_specified_company = () =>
            {
                _companyResponse.ShouldEqual("Succeeded");
                _customerResponse.ShouldEqual("Succeeded");
                _companySaved.Id.ShouldEqual(CompanyId);
                _customerSaved.Id.ShouldEqual(CustomerId);
                _customerSaved.CompanyId.ShouldEqual(CompanyId);
            };
    }
}

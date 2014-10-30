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

namespace OrdningsVaktRapport.Test.CustomerEntityTests
{
    class when_modifying_a_customer_with_name_already_exist
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly CustomerEntity Customer2 = new CustomerEntity();
        private static CustomerEntity _objectSaved;
        private static string _companyResponse;
        private static string _customerResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static readonly Guid CustomerId2 = Guid.NewGuid();
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
            Customer.Name = "Marie Lauvant";
            Customer.VisitationAddress = new Address { Street = "Hornsgatan 60", Postcode = "14180 Stockholm" };
            Customer.ManagerFirstname = "Manager Firstname";
            Customer.ManagerLastname = "Manager Lastname";
            _customerResponse = Repository.AddCustomer(Customer);
            Thread.Sleep(2000);

            Customer2.Id = CustomerId2;
            Customer2.CompanyId = CompanyId;
            Customer2.Name = "Kassim Diakite";
            Customer2.VisitationAddress = new Address { Street = "Hornsgatan 60", Postcode = "14180 Stockholm" };
            Customer2.ManagerFirstname = "Manager Firstname";
            Customer2.ManagerLastname = "Manager Lastname";
            _customerResponse = Repository.AddCustomer(Customer2);
            Thread.Sleep(2000);

        };

        private Because Of = () =>
        {
            _objectSaved = Repository.GetCustomerById(Customer);
            _objectSaved.Name = "Kassim Diakite";
            _exception = Catch.Exception(() => Repository.ModifyCustomer(_objectSaved));

        };

        private It should_modify_an_object_for_specified_company = () =>
        {
            _exception.Message.ShouldEqual("A customer with that name already exist");
        };
    }
}

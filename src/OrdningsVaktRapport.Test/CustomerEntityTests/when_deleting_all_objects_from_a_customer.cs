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
    class when_deleting_all_objects_from_a_customer
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static CustomerEntity _customerReturned = new CustomerEntity();
        private static CustomerEntity _customerReturnedAfterDelete = new CustomerEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static string _companyResponse;
        private static string _customerObjecResponse;


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
                Name = "Sture",
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
                Name = "Stures",
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

            _customerObjecResponse = Repository.AddObjectToCustomer(customerObject2);
            _customerReturned = Repository.GetCustomerById(Customer);
            _customerObjecResponse = Repository.DeleteAllCustomerObject(Customer);
            _customerReturnedAfterDelete = Repository.GetCustomerById(Customer);
        };

        private It should_delete_all_objects_from_a_customer = () =>
        {
            _customerReturned.Objects.Count.ShouldEqual(2);
            _customerReturnedAfterDelete.Objects.Count.ShouldBeLessThanOrEqualTo(0);
        };
    }
}

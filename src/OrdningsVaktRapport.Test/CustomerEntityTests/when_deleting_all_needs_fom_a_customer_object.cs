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
    class when_deleting_all_needs_fom_a_customer_object
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static CustomerEntity _customerReturned = new CustomerEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly CustomerObject CustomerObject = new CustomerObject();
        private static CustomerObject _savedCustomerObject;
        private static CustomerObject _customerObjectAfterDelete;
        private static readonly Need Need = new Need();
        private static Need _savedNeed;
        private static Need _modifiedNeed;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static readonly Guid CustomerObjectId = Guid.NewGuid();
        private static readonly Guid NeedId = Guid.NewGuid();
        private static string _companyResponse;
        private static string _customerObjecResponse;
        private static string _needResponse;

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

            CustomerObject.Id = CustomerObjectId;
            CustomerObject.Name = "Sture";
            CustomerObject.ResponsibleGuardFirstname = "ResponsibleGuardFirstname";
            CustomerObject.ResponsibleGuardLastname = "ResponsibleGuardLastname";
            CustomerObject.ResponsibleManagerFistname = "ResponsibleManagerFistname";
            CustomerObject.ResponsibleManagerLastname = "ResponsibleManagerLastname";
            CustomerObject.HourlyRate = 200;
            CustomerObject.License = true;
            CustomerObject.LicenseType = "LicenseType";
            CustomerObject.CompanyId = CompanyId;
            CustomerObject.CustomerId = CustomerId;
            _customerObjecResponse = Repository.AddObjectToCustomer(CustomerObject);
            var need = new Need();
            need.Id = Guid.NewGuid();
            need.NumberOfPersonalNeeded = "5";
            need.StartDateTime = DateTime.Now.AddMinutes(10);
            need.EndDateTime = DateTime.Now.AddMinutes(20);
            need.CustomerObjectId = CustomerObjectId;
            need.CustomerId = CustomerId;
            need.CompanyId = CompanyId;

            var need2 = new Need();
            need2.Id = Guid.NewGuid();
            need2.NumberOfPersonalNeeded = "5";
            need2.StartDateTime = DateTime.Now.AddMinutes(30);
            need2.EndDateTime = DateTime.Now.AddMinutes(40);
            need2.CustomerObjectId = CustomerObjectId;
            need2.CustomerId = CustomerId;
            need2.CompanyId = CompanyId;

            var need3 = new Need();
            need3.Id = Guid.NewGuid();
            need3.NumberOfPersonalNeeded = "5";
            need3.StartDateTime = DateTime.Now.AddMinutes(45);
            need3.EndDateTime = DateTime.Now.AddMinutes(50);
            need3.CustomerObjectId = CustomerObjectId;
            need3.CustomerId = CustomerId;
            need3.CompanyId = CompanyId;

            _needResponse = Repository.AddNeedToCustomerObject(need);
            _needResponse = Repository.AddNeedToCustomerObject(need2);
            _needResponse = Repository.AddNeedToCustomerObject(need3);
            _savedCustomerObject = Repository.GetCustomerObjectById(CustomerObject);
            _customerObjecResponse = Repository.DeleteAllNeedsFromCustomerObject(CustomerObject);
            _customerObjectAfterDelete = Repository.GetCustomerObjectById(CustomerObject);

        };

        private It should_delete_all_needs_from_a_customer_object = () =>
            {
                _savedCustomerObject.Needs.Count.ShouldEqual(3);
                _customerObjectAfterDelete.Needs.Count.ShouldBeLessThanOrEqualTo(0);
        };
    }
}

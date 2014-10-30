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
    class when_deleting_all_customer_from_a_company_that_dont_have_customer
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly CustomerEntity Customer2 = new CustomerEntity();
        private static CustomerEntity _objectSaved;
        private static CustomerEntity _objectDeleted;
        private static string _companyResponse;
        private static string _customerResponse;
        private static string _customerResponse2;
        private static string _deleteResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static readonly Guid CustomerId2 = Guid.NewGuid();
        private static Exception _exception;
        private static List<CustomerEntity> _objectListBeforeDelete;
        private static List<CustomerEntity> _objectListAfterDelete;

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
        };

        private Because Of = () =>
        {
            _objectListBeforeDelete = Repository.GetAllCustomer(Company);
            _deleteResponse = Repository.DeleteAllCustomer(Company);
            _objectListAfterDelete = Repository.GetAllCustomer(Company);
        };

        private It should_return_customer_list_is_empty_as_response = () =>
        {
            _deleteResponse.ShouldEqual("customer list is empty");
            
        };
    }
}

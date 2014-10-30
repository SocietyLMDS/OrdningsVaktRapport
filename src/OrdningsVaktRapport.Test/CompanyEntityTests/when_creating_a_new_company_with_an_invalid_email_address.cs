using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.CompanyEntityTests
{
    class when_creating_a_new_company_with_an_invalid_email_address
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static Guid _id = Guid.NewGuid();
        private static Exception _exception;

        private Establish Context = () =>
        {
            _company.Id = _id;
            _company.VisitationAddress = new Address { Street = "klockarvägen 15", Postcode = "14162 Huddinge" };
            _company.AuthorisationLink = "http://www.link.com";
            _company.EmailAddress = "d_ladji.hotmail.com";
            _company.ManagerFirstname = "Ladji";
            _company.ManagerLastname = "Diakite";
            _company.ManagerPersonalNumber = "197708090894";
            _company.Name = "Ladjis Vaktbolag";
            

        };

        private Because Of = () =>
        {
            _exception = Catch.Exception(() => _repository.AddCompany(_company));
        };

        private It an_exception_should_be_thrown = () =>
        {
            _exception.ShouldBeOfType(typeof(InvalidOperationException));
            _exception.Message.ShouldEqual("The email address is not a valid format");
        };
    }
}

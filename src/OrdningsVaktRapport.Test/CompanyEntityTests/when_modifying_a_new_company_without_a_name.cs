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
    class when_modifying_a_new_company_without_a_name
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static string _response;
        private static Exception exception;
        private static Guid _id = Guid.NewGuid();

        private Establish Context = () =>
        {
            _company.Id = _id;
            _company.VisitationAddress = new Address { Street = "klockarvägen 15", Postcode = "14162 Huddinge" };
            _company.AuthorisationLink = "http://www.link.com";
            _company.EmailAddress = "manager@hotmail.com";
            _company.ManagerFirstname = "Ladji";
            _company.ManagerLastname = "Diakite";
            _company.ManagerPersonalNumber = "197708090894";
            _company.Name = "Ladjis VaktBolag";
            _company.Password = "mypassword";
            _company.Username = "myusername";
            _response = _repository.AddCompany(_company);
        };

        private Because Of = () =>
        {
            var getCompany = _repository.GetCompanyById(_company);
            getCompany.ManagerFirstname = "Kassim";
            getCompany.ManagerLastname = "Olsson";
            getCompany.ManagerPersonalNumber = "198298090865";
            getCompany.Name = "";
            exception = Catch.Exception(() => _repository.ModifyCompany(getCompany));
        };

        private It an_exception_should_be_thrown = () =>
        {
            exception.Message.ShouldEqual("You cannot modify a new company without a name");
        };
    }
}

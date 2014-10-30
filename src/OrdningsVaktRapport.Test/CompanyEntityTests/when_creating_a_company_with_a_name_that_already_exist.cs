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

namespace OrdningsVaktRapport.Test.CompanyEntityTests
{
    class when_creating_a_company_with_a_name_that_already_exist
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static readonly CompanyEntity _company2 = new CompanyEntity();
        private static CompanyEntity CompanySaved;
        private static string _response;
        private static string _response2;
        private static Guid _id = Guid.NewGuid();
        private static Guid _id2 = Guid.NewGuid();
        private static Exception _exception;

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
            _response = _repository.AddCompany(_company);
            Thread.Sleep(2000);
        };

        private Because Of = () =>
        {
            _company2.Id = _id2;
            _company2.VisitationAddress = new Address { Street = "Flemingsgatan 15", Postcode = "15167 Huddinge" };
            _company2.AuthorisationLink = "http://www.testart.com";
            _company2.EmailAddress = "managersNew@hotmail.com";
            _company2.ManagerFirstname = "Kassims";
            _company2.ManagerLastname = "Seydou";
            _company2.ManagerPersonalNumber = "197708090894";
            _company2.Name = "Ladjis VaktBolag";
            _exception = Catch.Exception(() => _repository.AddCompany(_company2));
        };

        private It an_exception_should_be_thrown = () =>
        {
            _exception.Message.ShouldEqual("A company with that name already exist");
        };
    }
}

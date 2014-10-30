using System;
using System.Threading;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.CompanyEntityTests
{
    class when_creating_a_new_company_with_all_the_details
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static CompanyEntity CompanySaved;
        private static string _response;
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
                _response = _repository.AddCompany(_company);
            };

        private Because Of = () =>
            {
                CompanySaved = _repository.GetCompanyById(_company);
            };

        private It a_new_company_should_be_created = () =>
            {
                _response.ShouldEqual("Succeeded");
                CompanySaved.Id.ShouldEqual(_id);
                CompanySaved.VisitationAddress.Postcode.ShouldEqual("14162 Huddinge");
                CompanySaved.AuthorisationLink.ShouldEqual("http://www.link.com");
                CompanySaved.EmailAddress.ShouldEqual("manager@hotmail.com");
                CompanySaved.ManagerFirstname.ShouldEqual("Ladji");
                CompanySaved.ManagerLastname.ShouldEqual("Diakite");
                CompanySaved.ManagerPersonalNumber.ShouldEqual("197708090894");
                CompanySaved.Name.ShouldEqual("Ladjis VaktBolag");
            };
    }
}

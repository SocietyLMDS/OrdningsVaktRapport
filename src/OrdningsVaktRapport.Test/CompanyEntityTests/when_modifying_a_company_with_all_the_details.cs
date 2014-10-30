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
    class when_modifying_a_company_with_all_the_details
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static CompanyEntity CompanyModified;
        private static string _response;
        private static string _modifyResponce;
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
            var getCompany = _repository.GetCompanyById(_company);
            getCompany.ManagerFirstname = "Kassim";
            getCompany.ManagerLastname = "Olsson";
            getCompany.ManagerPersonalNumber = "198298090865";
            _modifyResponce = _repository.ModifyCompany(getCompany);
            CompanyModified = _repository.GetCompanyById(getCompany);
        };

        private It existing_company_should_be_modified = () =>
        {
            _modifyResponce.ShouldEqual("Succeeded");
            CompanyModified.Id.ShouldEqual(_id);
            CompanyModified.VisitationAddress.Postcode.ShouldEqual("14162 Huddinge");
            CompanyModified.AuthorisationLink.ShouldEqual("http://www.link.com");
            CompanyModified.EmailAddress.ShouldEqual("manager@hotmail.com");
            CompanyModified.ManagerFirstname.ShouldEqual("Kassim");
            CompanyModified.ManagerLastname.ShouldEqual("Olsson");
            CompanyModified.ManagerPersonalNumber.ShouldEqual("198298090865");
            CompanyModified.Name.ShouldEqual("Ladjis VaktBolag");
        };
    }
}

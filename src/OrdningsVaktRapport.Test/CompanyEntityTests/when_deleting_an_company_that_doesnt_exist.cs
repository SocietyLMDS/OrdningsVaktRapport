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
    class when_deleting_an_company_that_doesnt_exist
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static CompanyEntity CompanySaved;
        private static string _response;
        private static string _deleteResponse;
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
            //_response = _repository.AddCompany(_company);
        };

        private Because Of = () =>
        {
            _deleteResponse = _repository.DeleteCompany(_company);
        };

        private It should_return_the_response_to_UnSucceeded = () =>
        {

            _deleteResponse.ShouldEqual("UnSucceeded");
        };
    }
}

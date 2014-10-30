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
    class when_renewing_company_password_because_you_forgot_with_a_valid_email_address
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static  CompanyEntity _returnedCompany; 
        private static string _response;
        private static string _forgotResponse;
        private static Exception exception;
        private static Guid _id = Guid.NewGuid();

        private Establish Context = () =>
        {
            Company.Id = _id;
            Company.VisitationAddress = new Address { Street = "klockarvägen 15", Postcode = "14162 Huddinge" };
            Company.AuthorisationLink = "http://www.link.com";
            Company.EmailAddress = "manager@hotmail.com";
            Company.ManagerFirstname = "Ladji";
            Company.ManagerLastname = "Diakite";
            Company.ManagerPersonalNumber = "197708090894";
            Company.Name = "Ladjis VaktBolag";
            _response = Repository.AddCompany(Company);
            Thread.Sleep(2000);
        };

        private Because Of = () =>
            {
                _forgotResponse = Repository.ForgotPassword(Company.EmailAddress);
               _returnedCompany = Repository.GetCompanyById(Company);

            };

        private It an_exception_should_be_thrown = () =>
            {
                _forgotResponse.ShouldEqual("Succeeded");
                _returnedCompany.Password.ShouldEqual(_returnedCompany.Password);
            };
    }
}

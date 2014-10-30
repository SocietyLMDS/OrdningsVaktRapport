using System;
using System.Collections.Generic;
using System.Threading;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.CompanyEntityTests
{
    class when_retrieving_all_companies_in_an_unempty_company_database
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static readonly CompanyEntity _company2 = new CompanyEntity();
        private static List<CompanyEntity> _companyList;
        private static string _response;
        private static Guid _id = Guid.NewGuid();
        private static Guid _id2 = Guid.NewGuid();
       
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

            _company2.Id = _id2;
            _company2.VisitationAddress = new Address { Street = "Visättravägen 15", Postcode = "14151 Huddinge" };
            _company2.AuthorisationLink = "http://www.links.com";
            _company2.EmailAddress = "managerladji@hotmail.com";
            _company2.ManagerFirstname = "Kassim";
            _company2.ManagerLastname = "Diakite";
            _company2.ManagerPersonalNumber = "198208090894";
            _company2.Name = "Kassims VaktBolag";
            _company2.Password = "mypasswordkassim";
            _company2.Username = "myusernamekassim";

            _response = _repository.AddCompany(_company);
            _response = _repository.AddCompany(_company2);
            Thread.Sleep(2000);
        };

        private Because Of = () =>
            {
                _companyList = _repository.GetAllCompanies();
            };

        private It company_list_count_should_be_creater_than_0 = () =>
        {
            _companyList.Count.ShouldBeGreaterThan(0);

        };
    }
}

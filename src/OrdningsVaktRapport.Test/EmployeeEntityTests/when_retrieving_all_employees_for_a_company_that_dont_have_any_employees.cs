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

namespace OrdningsVaktRapport.Test.EmployeeEntityTests
{
    class when_retrieving_all_employees_for_a_company_that_dont_have_any_employees
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static string _companyResponse;
        private static List<EmployeeEntity> _employeeList;
        private static readonly Guid CompanyId = Guid.NewGuid();

        private Establish Context = () =>
        {
            Company.Id = CompanyId;
            Company.Name = "Svea Vaktbolag";
            Company.VisitationAddress = new Address { Street = "Sveavägen 14", Postcode = "15161 Stockholm" };
            Company.ManagerFirstname = "Ladji";
            Company.ManagerLastname = "diakite";
            Company.ManagerPersonalNumber = "197708090894";
            Company.EmailAddress = "newemailaddress@hotmail.com";
            Company.AuthorisationLink = "http://www.link.com";
            _companyResponse = Repository.AddCompany(Company);
            Thread.Sleep(2000);

          
        };

        private Because Of = () =>
        {
            _employeeList = Repository.GetAllEmployee(Company);
        };

        private It should_return_an_empty_employee_list_for_a_company = () =>
            {
                _employeeList.Count.ShouldEqual(0);
            };
    }
}

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
    class when_deleting_all_employees_from_a_company_that_dont_have_any_employees
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static readonly EmployeeEntity Employee1 = new EmployeeEntity();
        private static readonly EmployeeEntity Employee2 = new EmployeeEntity();
        private static string _companyResponse;
        private static string _employeeResponse;
        private static string _deleteResponse;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid Employee1Id = Guid.NewGuid();
        private static readonly Guid Employee2Id = Guid.NewGuid();
        private static List<EmployeeEntity> _employeeListBeforeDelete;
        private static List<EmployeeEntity> _employeeListAfterDelete;

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
            _deleteResponse = Repository.DeleteAllEmployee(Company);

        };

        private It should_return_Employee_list_is_empty_as_response = () =>
            {
                _deleteResponse.ShouldEqual("Employee list is empty");
            };
    }
}

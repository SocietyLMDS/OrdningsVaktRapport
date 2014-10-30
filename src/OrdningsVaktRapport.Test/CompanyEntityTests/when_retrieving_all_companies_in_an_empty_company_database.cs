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
    class when_retrieving_all_companies_in_an_empty_company_database
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
           
        };

        private Because Of = () =>
        {
            _companyList = _repository.GetAllCompanies();
        };

        private It should_return_company_list_as_null = () =>
        {
            _companyList.Count.ShouldBeLessThanOrEqualTo(0);

        };
    }
}

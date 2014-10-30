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
    class when_deleting_a_company_that_does_not_exist
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static string _deleteResponse;
       

        private Establish Context = () =>
        {
          
        };

        private Because Of = () =>
            {
                var company = new CompanyEntity {Id = Guid.NewGuid()};
                _deleteResponse = _repository.DeleteCompany(company);
      
        };

        private It should_should_return_unsucceeded_as_response = () =>
        {
            _deleteResponse.ShouldEqual("UnSucceeded");
        };
    }
}

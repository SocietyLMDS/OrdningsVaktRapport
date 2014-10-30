using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.CustomerEntityTests
{
    class when_deleting_a_customer_that_does_not_exist
    {
        private static readonly Store _store = new Store();
        private static readonly IRepository _repository = new Repository(_store);
        private static string _deleteResponse;


        private Establish Context = () =>
        {

        };

        private Because Of = () =>
        {
            var customer = new CustomerEntity { Id = Guid.NewGuid(), CompanyId = Guid.NewGuid()};
            _deleteResponse = _repository.DeleteCustomer(customer);

        };

        private It should_should_return_unsucceeded_as_response = () =>
        {
            _deleteResponse.ShouldEqual("UnSucceeded");
        };
    }
}

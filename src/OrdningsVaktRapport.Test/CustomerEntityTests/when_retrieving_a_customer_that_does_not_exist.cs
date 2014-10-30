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

namespace OrdningsVaktRapport.Test.CustomerEntityTests
{
    class when_retrieving_a_customer_that_does_not_exist
    {

        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static CustomerEntity _object = new CustomerEntity();
      
    

        private Establish Context = () =>
        {

        };

        private Because Of = () =>
            {
                var objects = new CustomerEntity {Id = Guid.NewGuid(), CompanyId = Guid.NewGuid()};
                _object = Repository.GetCustomerById(objects);
        };

        private It should_return_a_null_object = () =>
        {
            _object.ShouldBeNull();
        };
    }
}

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
    class when_retrieving_all_customer_for_a_company_that_have_no_customers
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static List<CustomerEntity> _objectList;

        private Establish Context = () =>
        {
            

        };

        private Because Of = () =>
            {
                var company = new CompanyEntity {Id = Guid.NewGuid()};
                _objectList = Repository.GetAllCustomer(company);
        };

        private It should_return_an_empty_object_list = () =>
            {
                _objectList.Count.ShouldEqual(0);

            };
    }
}

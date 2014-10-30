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
    class when_deleting_an_employee_that_does_not_exist
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static string _employeeDeleteResponse;
      

        private Establish Context = () =>
        {

        };

        private Because of = () =>
            {
                var employee = new EmployeeEntity {Id = Guid.NewGuid(), CompanyId = Guid.NewGuid()};
                _employeeDeleteResponse = Repository.DeleteEmployee(employee);
            };

        private It should_should_respond_with_an_unsucceeded_message = () =>
        {
            _employeeDeleteResponse.ShouldEqual("UnSucceeded");
        };
    }
}

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
    class when_retrieving_an_employee_that_does_not_exist_in_the_database
    {

        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static EmployeeEntity _employeeSaved = new EmployeeEntity();

        private Establish Context = () =>
        {
            
        };

        private Because of = () =>
            {
                var employee = new EmployeeEntity {Id = Guid.NewGuid(), CompanyId = Guid.NewGuid()};
            _employeeSaved = Repository.GetEmployeeById(employee);
        };

        private It should_return_a_null_employee_entity = () =>
        {
            _employeeSaved.ShouldBeNull();
        };
    }
}

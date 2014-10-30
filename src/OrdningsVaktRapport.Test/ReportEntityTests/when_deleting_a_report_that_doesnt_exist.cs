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

namespace OrdningsVaktRapport.Test.ReportEntityTests
{
    class when_deleting_a_report_that_doesnt_exist
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static string _reponse;
        private static ReportEntity _report;
      
        private Establish Context = () =>
        {
            _report = new ReportEntity { Id = Guid.NewGuid(), CompanyId = Guid.NewGuid() };
        };

        private Because Of = () =>
        {
            _reponse = Repository.DeleteReport(_report);
            
        };

        private It Should_return_unsucceeded_as_response = () =>
        {
            _reponse.ShouldEqual("UnSucceeded");
        };
    }
}

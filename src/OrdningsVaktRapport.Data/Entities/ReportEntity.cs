using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Entities
{
    public partial class ReportEntity
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerObjectId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerObjectName { get; set; }
        public string EmployeeFirstname { get; set; }
        public string EmployeeLastname { get; set; }
        public string ReportName { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportModel ReportModel { get; set; }
    }
}

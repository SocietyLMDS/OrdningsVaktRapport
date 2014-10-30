using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Entities
{
    public class ScheduleEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CustomerObjectId { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerObjectName { get; set; }
        public Address CustomerObjectAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Shift> Schedules { get; set; }
    }
}

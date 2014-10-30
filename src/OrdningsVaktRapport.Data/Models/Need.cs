using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public class Need
    {
        public Guid Id { get; set; }
        public Guid CustomerObjectId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public string NumberOfPersonalNeeded { get; set; }
        public DateTime  StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string StartDateTimeCombined { get; set; }
        public string EndDateTimeCombined { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public class TimesheetObject
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TotalTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public class ReportModel
    {
        public Boolean Avv { get; set; }
        public Boolean Avl { get; set; }
        public Omh Omh { get; set; }
    }
}

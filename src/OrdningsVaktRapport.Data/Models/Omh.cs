using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public class Omh
    {
        public bool SkyddsVisitation { get; set; }
        public bool Fangsel { get; set; }
        public string Protocol { get; set; }
        public string Anledning { get; set; }
    }
}

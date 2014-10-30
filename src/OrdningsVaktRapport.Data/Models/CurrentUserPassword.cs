using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public class CurrentUserPassword
    {
        public string UserType { get; set; }
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RetypeNewPassword { get; set; }
    }
}

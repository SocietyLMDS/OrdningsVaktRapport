using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public interface IUsernamePassword
    {
        string Username { get; set; }
        string Password { get; set; }
        Guid Id { get; set; }
    }
}

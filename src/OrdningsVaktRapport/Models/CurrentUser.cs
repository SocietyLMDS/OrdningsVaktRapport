using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdningsVaktRapport.Models
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string UserType { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EmailAddress { get; set; }
    }
}
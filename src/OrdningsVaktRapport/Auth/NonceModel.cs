using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdningsVaktRapport.Auth
{
    public class NonceModel
    {
        public int NonceCount { get; set; }
        public DateTime NonceExpirationDate { get; set; }
    }
}
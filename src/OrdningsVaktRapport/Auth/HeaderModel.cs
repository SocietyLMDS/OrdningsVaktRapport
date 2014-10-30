using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdningsVaktRapport.Auth
{
    public class HeaderModel
    {
        public string Nonce { get; set; }
        public string CNonce { get; set; }
        public string NonceCount { get; set; }
        public string Realm { get; set; }
        public string UserName { get; set; }
        public string Uri { get; set; }
        public string Response { get; set; }
        public string Method { get; set; }
        public string QoP { get; set; }
    }
}
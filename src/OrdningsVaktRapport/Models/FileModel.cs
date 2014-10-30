using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdningsVaktRapport.Models
{
    public class FileModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
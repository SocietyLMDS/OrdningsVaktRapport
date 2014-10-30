using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string OrganisationNumber { get; set; }
        public Address VisitationAddress { get; set; }
        public Address PostalAddress { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteLink { get; set; }
        public string FTaxLink { get; set; }
        public string ManagerFirstname { get; set; }
        public string ManagerLastname { get; set; }
        public List<CustomerObject> Objects { get; set; }
    }
}

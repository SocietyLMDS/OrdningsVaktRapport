using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Entities
{
    public class EmployeeEntity : IUsernamePassword
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string JobDescription { get; set; }
        public string EmploymentNumber { get; set; }
        public string PersonalNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Address Address { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FTaxLink { get; set; }
        public string BankAccount { get; set; }
        public string DrivingLicenseAndIdLink { get; set; }
        public string EducationLicenseLink { get; set; }
        public string SecurityLicenseLink { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
  }

}

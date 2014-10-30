using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdningsVaktRapport.Auth
{
    public static class CustomClaims
    {
        public static string Id = "Id";
        public static string CompanyId = "CompanyId";
        public static string CompanyName = "CompanyName";
        public static string CurrentUser = "CurrentUser";
        public static string Firstname = "Firstname";
        public static string Lastname = "Lastname";
        public static string EmailAddress = "EmailAddress";
    }
}
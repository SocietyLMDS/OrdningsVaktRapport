using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Models;

namespace OrdningsVaktRapport.Data.Utils
{
   public class SendEmail
    {
       public static string Send(string email,  string subject, string message)
       {
           try
           {
               var client = new SmtpClient();
               client.Port = 587;
               client.Host = "smtp.gmail.com";
               client.EnableSsl = true;
               client.Timeout = 10000;
               client.DeliveryMethod = SmtpDeliveryMethod.Network;
               client.UseDefaultCredentials = false;
               client.Credentials = new NetworkCredential("diakiteladji@gmail.com", "ledge177");
               var mm = new MailMessage("diakiteladji@gmail.com", email, subject, message);
               mm.BodyEncoding = UTF8Encoding.UTF8;
               mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
               client.Send(mm);
               return "succeeded";
           }
           catch (Exception e)
           {
               return "unsucceeded";

           }
       }
    }
}

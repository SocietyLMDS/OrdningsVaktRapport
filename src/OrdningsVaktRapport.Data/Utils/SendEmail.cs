using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using  SendGrid;
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
               //var client = new SmtpClient();
               //client.Port = 587;
               //client.Host = "smtp.gmail.com";
               //client.EnableSsl = true;
               //client.Timeout = 10000;
               //client.DeliveryMethod = SmtpDeliveryMethod.Network;
               //client.UseDefaultCredentials = false;
               //client.Credentials = new NetworkCredential("diakiteladji@gmail.com", "ledge177");
               //var mm = new MailMessage("diakiteladji@gmail.com", email, subject, message);
               //mm.BodyEncoding = UTF8Encoding.UTF8;
               //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
               //client.Send(mm);

               var emailMessage = new SendGridMessage();
               emailMessage.AddTo(email);
               emailMessage.From = new MailAddress("diakieladji@gmail.com","over/out");
               emailMessage.Subject = subject;
               emailMessage.Text = message;
               var credentials = new NetworkCredential("azure_e046cabf6b653cd0dc663d74514e6f36@azure.com", "kFE691o8o0l7U8n");
               var transportWeb = new Web(credentials);
               transportWeb.Deliver(emailMessage);
               return "succeeded";
           }
           catch (Exception e)
           {
               return "unsucceeded";

           }
       }
    }
}

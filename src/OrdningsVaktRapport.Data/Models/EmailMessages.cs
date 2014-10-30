using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdningsVaktRapport.Data.Models
{
    public static class EmailMessages
    {
        public static string UserCreatedMessage(string firstname, string lastname, string message, string username, string password)
        {
            return "Hi "+firstname+" "+lastname+"\n\n"+ message + "Username: " + username + "\nPassword: " + password + "\n\nVisit http://www.overout.com to login \nYou have the option to change your user credentials once you've logged in\n\nRegards OVER/OUT";
        }

        public static string ShifNotificationMessage(string firstname, string lastname, string message, string customerObjectName, Address address, DateTime startTime, DateTime endTime)
        {
            return "Hi " + firstname + " " + lastname + "\n\n" + message + "Place: "+customerObjectName+"\n\nAddress: "+address.Street+" "+address.Postcode+"\n\nStart Date: "+startTime.ToShortDateString()+"\nStart Time: "+startTime.ToShortTimeString()+"\n\nEnd Date: "+ endTime.ToShortDateString()+"\nEnd Time: "+ endTime.ToShortTimeString()+ "\n\nVisit http://www.overout.com to login \n so you can confirm if you can work";
        }

        public static string ShiftStatusChangedNotification(string managerFirstname, string managerLastName, string employeeFirstName, string employeeLastName, Address address, DateTime startTime, DateTime enTime, string objectName, string status, string message)
        {
            return "Hi " + managerFirstname + " " + managerLastName + "\n\n" + message + "Object: "+objectName+"\n\nAddress: "+address.Street+" "+address.Postcode+"\n\nEmployee: "+employeeFirstName +" "+employeeLastName+"\n\nStart Time: "+startTime.ToShortTimeString()+"\n\nEnd Time: "+enTime.ToShortTimeString()+"\n\nStatus: "+status;
        }
    }
}

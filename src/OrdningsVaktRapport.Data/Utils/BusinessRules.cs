using System;
using System.Data;
using System.Text.RegularExpressions;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;
using Raven.Client.Document;
using System.Linq;
using System.Linq.Expressions;

namespace OrdningsVaktRapport.Data.Utils
{
    class BusinessRules
    {
        public void CompanyAddAndModifyRule(CompanyEntity company, Store store, string message)
        {
            if (string.IsNullOrEmpty(company.Name)) throw new InvalidOperationException("You cannot " + message + " a company without a name");
            if (string.IsNullOrEmpty(company.EmailAddress)) throw new InvalidOperationException("You cannot " + message + " a company without an email address");

            if (message == "add")
            {
                if (string.IsNullOrEmpty(company.Password)) throw new InvalidOperationException("You cannot " + message + " a company without a password");
                if (string.IsNullOrEmpty(company.Username)) throw new InvalidOperationException("You cannot " + message + " a company without a username");
                if (company.Password.Length < 6) throw new InvalidOperationException("A password cannot be chorter then 6 characters");
                if (company.Username.Length < 6) throw new InvalidOperationException("A username cannot be chorter then 6 characters");
            }

            if (!IsValidEmailAddress(company.EmailAddress)) throw new InvalidOperationException("The email address is not a valid format");

            var emailCheck = store.GetCompanyByEmail(company);
            if (emailCheck != null)
            {
                if (emailCheck.Id != company.Id)
                {
                    throw new InvalidOperationException("A company with that email address already exist");
                }
            }

            var usernameCheck = store.GetCompanyByName(company);
            if (usernameCheck != null)
            {
                if (usernameCheck.Id != company.Id)
                {
                    throw new InvalidOperationException("A company with that name already exist");
                }
            }

            var checkEmployeeEmail = store.CheckIfEmployeeEmailExist(company.EmailAddress);
            if (checkEmployeeEmail != null)
            {
                throw new InvalidOperationException("somebody with that email address already exist");
            }

        }

        public void EmployeeAddAndModifyRules(EmployeeEntity employee, Store store, string message)
        {
            if (string.IsNullOrEmpty(employee.Firstname)) throw new InvalidOperationException("You cannot " + message + " an employee without a first name");
            if (string.IsNullOrEmpty(employee.Lastname)) throw new InvalidOperationException("You cannot " + message + " an employee without a last name");
            if (string.IsNullOrEmpty(employee.EmailAddress)) throw new InvalidOperationException("You cannot " + message + " an employee without an email address");
            if (message == "add")
            {
                if (string.IsNullOrEmpty(employee.Password)) throw new InvalidOperationException("You cannot " + message + " an employee without a password");
                if (string.IsNullOrEmpty(employee.Username)) throw new InvalidOperationException("You cannot " + message + " an employee without a username");
                if (employee.Password.Length < 6) throw new InvalidOperationException("A password cannot be chorter then 6 characters");
                if (employee.Username.Length < 6) throw new InvalidOperationException("A username cannot be chorter then 6 characters");
            }
            if (!IsValidEmailAddress(employee.EmailAddress)) throw new InvalidOperationException("The email address is not a valid format");

            var emailCheck = store.GetEmployeeByEmail(employee);

            if (emailCheck != null)
            {
                if (emailCheck.Id != employee.Id)
                {
                    throw new InvalidOperationException("An employee with that email address already exist");
                }
            }

            var checkCompanyEmail = store.CheckIfCompanyEmailExist(employee.EmailAddress);
            if (checkCompanyEmail != null)
            {
                throw new InvalidOperationException("somebody with that email address already exist");
            }
        }

        public void CustomerAddAndModifyRules(CustomerEntity customerEntity, Store store, string message)
        {
            if (string.IsNullOrEmpty(customerEntity.Name)) throw new InvalidOperationException("You cannot " + message + " a new customer without a name");

            var customerNameCheck = store.GetCustomerByName(customerEntity);

            if (customerNameCheck != null)
            {
                if (customerNameCheck.Id != customerEntity.Id)
                {
                    throw new InvalidOperationException("A customer with that name already exist");
                }
            }

        }

        public void ReportAddAndModifyRules(ReportEntity report)
        {
            if (report.ReportName.ToLower() == "pl13")
            {
                if (!report.ReportModel.Avl && !report.ReportModel.Avv)
                {
                    if (report.ReportModel.Omh.Fangsel && string.IsNullOrEmpty(report.ReportModel.Omh.Protocol))
                    {
                        throw new InvalidOperationException("You've chosen true that somebody was jailed but forgot to include a protocol");
                    }
                }
            }

            if (report.ReportName.ToLower() == "grip")
            {
                if (report.ReportModel.Omh.Fangsel && string.IsNullOrEmpty(report.ReportModel.Omh.Protocol))
                {
                    throw new InvalidOperationException("You've chosen true that somebody was jailed but forgot to include a protocol");
                }

                if (report.ReportModel.Omh.SkyddsVisitation && string.IsNullOrEmpty(report.ReportModel.Omh.Anledning))
                {
                    throw new InvalidOperationException("You've chosen false that somebody was put on protection visitation but forgot to include a reason");
                }
            }

        }

        public void ScheduleAddAndModifyRules(ScheduleEntity schedule, Store store, string message)
        {
            if (schedule.StartDate.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a new schedule without a start date");
            if (schedule.EndDate.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a new schedule without a end date");
            if (schedule.StartDate < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a new schedule with a start date that's in the pass");
            if (schedule.StartDate < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a new schedule with a end date that's in the pass");
            if (schedule.EndDate < schedule.StartDate) throw new InvalidOperationException("You cannot " + message + " a new schedule with a end date that's less than the start date");

            var checkIfScheduleExist = store.GetScheduleByStartAndEndDate(schedule);

            if (checkIfScheduleExist != null)
            {
                if (checkIfScheduleExist.Id != schedule.Id)
                {
                    throw new InvalidOperationException("A schedule already exist with the dates you have chosen");
                }
            }
        }

        public void ShiftAddAndModifyRules(Shift shift, Store store, string message)
        {
            if (shift.StartTime.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a shift without a start time");
            if (shift.EndTime.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a shift without a end time");
            if (shift.StartTime < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a shift with a start time that's in the pass");
            if (shift.EndTime < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a shift with a end time that's in the pass");
            if (shift.EndTime < shift.StartTime) throw new InvalidOperationException("You cannot " + message + " a shift with a end time that's less than the start time");

            var checkIfShiftExist = store.GetShiftByEmployee(shift);
            if (checkIfShiftExist != null)
            {
                if (checkIfShiftExist.Id != shift.Id)
                {
                    throw new InvalidOperationException("The employee you have chosen is unavailable for the date chosen");
                }
            }
        }

        public void CustomerObjectAddAndModifyRules(CustomerObject customerObject, Store store, string message)
        {
            if (string.IsNullOrEmpty(customerObject.Name)) throw new InvalidOperationException("You cannot " + message + " an object without a name");

            var customerObjectCheck = store.GetCustomerObjectByName(customerObject);

            if (customerObjectCheck != null)
            {
                if (customerObjectCheck.Id != customerObject.Id)
                {
                    throw new InvalidOperationException("An object with that name already exist");
                }
            }
        }

        public void CustomerObjectNeedAddAndModifyRules(Need need, Store store, string message)
        {
            if (string.IsNullOrEmpty(need.NumberOfPersonalNeeded)) throw new InvalidOperationException("You cannot " + message + " a need without specifying number of personnel needed");
            if (need.StartDateTime.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a need without a start date time");
            if (need.EndDateTime.Ticks <= 0) throw new InvalidOperationException("You cannot " + message + " a need without a end date time");
            if (need.StartDateTime < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a need with a start date time that's in the pass");
            if (need.EndDateTime < DateTime.Now) throw new InvalidOperationException("You cannot " + message + " a need with a end date time that's in the pass");
            if (need.EndDateTime < need.StartDateTime) throw new InvalidOperationException("You cannot " + message + " a need with a end date time that's less than the start time");

            var needStartDateTime = store.GetNeedFromCustomerObjectByStartDateTime(need);
            var needEndDateTime = store.GetNeedFromCustomerObjectByEndDateTime(need);

            if (needStartDateTime != null)
            {
                if (needStartDateTime.Id != need.Id)
                {
                    throw new InvalidOperationException("There's already a need for the start time you've entered");
                }

            }

            if (needEndDateTime != null)
            {
                if (needEndDateTime.Id != need.Id)
                {
                    throw new InvalidOperationException("There's already a need for the end time you've entered");
                }

            }
        }

        public void ForgotPasswordRules(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new InvalidOperationException("Please enter an email address");
            if (!IsValidEmailAddress(email)) throw new InvalidOperationException("The email address is not a valid format");
        }

        private bool IsValidEmailAddress(string email)
        {
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email) && !email.EndsWith(".");
        }

        public void ChangePasswordRules(CurrentUserPassword currentUser)
        {
            if (string.IsNullOrEmpty(currentUser.OldPassword)) throw new InvalidOperationException("The old password field is empty");
            if (currentUser.OldPassword.Length < 6) throw new InvalidOperationException("The old password cannot be shorter then 6 characters");
            if (string.IsNullOrEmpty(currentUser.NewPassword)) throw new InvalidOperationException("The new password field is empty");
            if (currentUser.NewPassword.Length < 6) throw new InvalidOperationException("The new password cannot be shorter then 6 characters");
            if (currentUser.NewPassword != currentUser.RetypeNewPassword) throw new InvalidOperationException("The passwords do not match");
            if (currentUser.NewPassword == currentUser.OldPassword) throw new InvalidOperationException("The new password cannot be the same as the old password");

        }
    }
}

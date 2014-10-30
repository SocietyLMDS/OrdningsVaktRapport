﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Models;
using OrdningsVaktRapport.Data.Services;

namespace OrdningsVaktRapport.Test.ReportEntityTests
{
    class when_deleting_all_reports_from_a_company_that_has_reports
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static EmployeeEntity _employee = new EmployeeEntity();
        private static CustomerEntity _customer = new CustomerEntity();
        private static ReportEntity _report = new ReportEntity();
        private static ReportEntity _reportSaved;
        private static string _reponse;
        private static string _deleteResponse;
        private static Guid _companyId = Guid.NewGuid();
        private static Guid _employeeId = Guid.NewGuid();
        private static Guid _customerId = Guid.NewGuid();
        private static Guid _reportId = Guid.NewGuid();
        private static List<ReportEntity> _reportsBeforeDelete;
        private static List<ReportEntity> _reportsAfterDelete;

        private Establish Context = () =>
        {
            _company.Id = _companyId;
            _company.OrganisationNumber = "2090901901920";
            _company.Name = "Ladjis Vaktbolag";
            _company.VisitationAddress = new Address { Street = "Birgerjarlsvägen 15", Postcode = "15142 Stockholm" };
            _company.EmailAddress = "Sture@sturecompaniet.com";
            _company.AuthorisationLink = "http://www.link.com";
            _company.ManagerFirstname = "Firsname";
            _company.ManagerLastname = "Lastname";
            _company.ManagerPersonalNumber = "77080809097";
            _reponse = Repository.AddCompany(_company);
            Thread.Sleep(2000);

            _employee.Id = _employeeId;
            _employee.CompanyId = _companyId;
            _employee.Firstname = "EmployeeFirstname";
            _employee.Lastname = "EmployeeLastname";
            _employee.PersonalNumber = "8798358935059";
            _employee.Address = new Address { Street = "Employee Street", Postcode = "11256 Huddinge" };
            _employee.EmailAddress = "d_ladji@hotmail.com";
            _employee.DrivingLicenseAndIdLink = "http://www.idlink.com";
            _employee.EducationLicenseLink = "http://www.educationLink.com";
            _employee.SecurityLicenseLink = "http://www.securitylink.com";
            _employee.BankAccount = "899839238920";
            _employee.HourlyRate = 200;
            _employee.MobileNumber = "99067969709";
            _reponse = Repository.AddEmployee(_employee);
            Thread.Sleep(2000);

            _customer.Id = _customerId;
            _customer.CompanyId = _companyId;
            _customer.Name = "Sture Companiet";
            _customer.VisitationAddress = new Address { Street = "Vissgatan 17", Postcode = "2372 stockholm" };
            _customer.ManagerFirstname = "ObjectManager";
            _customer.ManagerLastname = "object manager lastname";
            _reponse = Repository.AddCustomer(_customer);
            Thread.Sleep(2000);

            _report.Id = Guid.NewGuid();
            _report.CompanyId = _companyId;
            _report.EmployeeId = _employeeId;
            _report.CustomerId = _customerId;
            _report.ReportName = "FBS";
            _report.ReportModel = new ReportModel { Avl = true, Avv = true };
            _reponse = Repository.AddReport(_report);

            _report.Id = Guid.NewGuid();
            _report.CompanyId = _companyId;
            _report.EmployeeId = _employeeId;
            _report.CustomerId = _customerId;
            _report.ReportName = "PL13";
            _report.ReportModel = new ReportModel { Avl = true, Avv = true, Omh = new Omh { SkyddsVisitation = false, Fangsel = true, Protocol = "Protocol" } };
            _reponse = Repository.AddReport(_report);

            _report.Id = Guid.NewGuid();
            _report.CompanyId = _companyId;
            _report.EmployeeId = _employeeId;
            _report.CustomerId = _customerId;
            _report.ReportName = "GRIP";
            _report.ReportModel = new ReportModel { Avl = true, Avv = true, Omh = new Omh { SkyddsVisitation = true, Fangsel = true, Protocol = "Protocol", Anledning = "Anledning" } };
            _reponse = Repository.AddReport(_report);

            Thread.Sleep(2000);

        };

        private Because Of = () =>
        {
            _reportsBeforeDelete = Repository.GetAllReports(_company);
            _deleteResponse = Repository.DeleteAllReport(_company);
            _reportsAfterDelete = Repository.GetAllReports(_company);
        };

        private It Should_delete_all_reports_from_a_company = () =>
        {
            _reportsBeforeDelete.Count.ShouldBeGreaterThan(0);
            var fbs = _reportsBeforeDelete.ElementAt(0);
            var pl13 = _reportsBeforeDelete.ElementAt(1);
            var grip = _reportsBeforeDelete.ElementAt(2);
            fbs.ReportName.ShouldEqual("FBS");
            pl13.ReportName.ShouldEqual("PL13");
            grip.ReportName.ShouldEqual("GRIP");

            _deleteResponse.ShouldEqual("Succeeded");
            _reportsAfterDelete.Count.ShouldBeLessThanOrEqualTo(0);

        };
    }
}
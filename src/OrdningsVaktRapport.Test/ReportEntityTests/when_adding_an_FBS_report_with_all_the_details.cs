using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrdningsVaktRapport.Data.Entities;
using OrdningsVaktRapport.Data.Services;
using OrdningsVaktRapport.Data.Models;
using Machine.Specifications;

namespace OrdningsVaktRapport.Test.ReportEntityTests
{
    class when_adding_an_FBS_report_with_all_the_details
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity _company = new CompanyEntity();
        private static EmployeeEntity _employee = new EmployeeEntity();
        private static CustomerEntity _customer = new CustomerEntity();
        private static ReportEntity _report = new ReportEntity();
        private static ReportEntity _reportSaved;
        private static string _reponse;
        private static Guid _companyId = Guid.NewGuid();
        private static Guid _employeeId = Guid.NewGuid();
        private static Guid _customerId = Guid.NewGuid();
        private static Guid _reportId = Guid.NewGuid();

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
                _employee.Address = new Address {Street = "Employee Street", Postcode = "11256 Huddinge"};
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
                _customer.VisitationAddress = new Address {Street = "Vissgatan 17", Postcode = "2372 stockholm"};
                _customer.ManagerFirstname = "ObjectManager";
                _customer.ManagerLastname = "object manager lastname";
                _reponse = Repository.AddCustomer(_customer);
                Thread.Sleep(2000);

                _report.Id = _reportId;
                _report.CompanyId = _companyId;
                _report.EmployeeId = _employeeId;
                _report.CustomerId = _customerId;
                _report.ReportName = "FBS";
                _report.ReportModel = new ReportModel {Avl = true, Avv = true};
                _reponse = Repository.AddReport(_report);
                Thread.Sleep(2000);

            };

        private Because Of = () =>
            {
                _reportSaved = Repository.GetReportById(_report);
            };

        private It Should_add_an_FBS_Report = () =>
            {
                var company = Repository.GetCompanyById(new CompanyEntity {Id = _reportSaved.CompanyId});
                var employee = Repository.GetEmployeeById(new EmployeeEntity {Id = _reportSaved.EmployeeId, CompanyId = _reportSaved.CompanyId});
                var customer = Repository.GetCustomerById(new CustomerEntity { Id = _reportSaved.CustomerId, CompanyId = _reportSaved.CompanyId });

                company.OrganisationNumber.ShouldEqual("2090901901920");
                employee.PersonalNumber.ShouldEqual("8798358935059");
                customer.Name.ShouldEqual("Sture Companiet");

                _reportSaved.Id.ShouldEqual(_reportId);
                _reportSaved.ReportName.ShouldEqual("FBS");
                _reportSaved.ReportModel.Avl.ShouldEqual(true);
                _reportSaved.ReportModel.Avv.ShouldEqual(true);
                _reportSaved.ReportModel.Omh.ShouldBeNull();
            };
    }
}

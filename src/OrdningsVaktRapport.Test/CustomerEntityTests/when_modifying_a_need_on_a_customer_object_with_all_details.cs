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

namespace OrdningsVaktRapport.Test.CustomerEntityTests
{
    class when_modifying_a_need_on_a_customer_object_with_all_details
    {
        private static readonly Store Store = new Store();
        private static readonly IRepository Repository = new Repository(Store);
        private static readonly CompanyEntity Company = new CompanyEntity();
        private static CustomerEntity _customerReturned = new CustomerEntity();
        private static readonly CustomerEntity Customer = new CustomerEntity();
        private static readonly CustomerObject CustomerObject = new CustomerObject();
        private static CustomerObject _savedCustomerObject;
        private static readonly Need Need = new Need();
        private static Need _savedNeed;
        private static Need _modifiedNeed;
        private static readonly Guid CompanyId = Guid.NewGuid();
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static readonly Guid CustomerObjectId = Guid.NewGuid();
        private static readonly Guid NeedId = Guid.NewGuid();
        private static string _companyResponse;
        private static string _customerObjecResponse;
        private static string _needResponse;
        private static DateTime _startDateTime;
        private static DateTime _endDateTime;
        private static DateTime _newStartDateTime;
        private static DateTime _newEndDateTime;

        private Establish Context = () =>
        {
            _endDateTime = DateTime.Now.AddMinutes(15);
            _endDateTime = DateTime.Now.AddMinutes(30);
            _newStartDateTime = DateTime.Now.AddMinutes(40);
            _newEndDateTime = DateTime.Now.AddMinutes(50);

            Company.Id = CompanyId;
            Company.Name = "Sverige VaktBolag";
            Company.VisitationAddress = new Address { Street = "Visättavägen 20", Postcode = "14161 Huddinge" };
            Company.ManagerFirstname = "Ladji";
            Company.ManagerLastname = "Diakite";
            Company.ManagerPersonalNumber = "7708090894";
            Company.EmailAddress = "ladji@gmail.com";
            Company.AuthorisationLink = "http://www.link.se";
            _companyResponse = Repository.AddCompany(Company);
            Thread.Sleep(2000);

            Customer.Id = CustomerId;
            Customer.CompanyId = CompanyId;
            Customer.Name = "Marie Lauvaut";
            Customer.VisitationAddress = new Address { Street = "Hornsgatan 60", Postcode = "14180 Stockholm" };
            Customer.ManagerFirstname = "Manager Firstname";
            Customer.ManagerLastname = "Manager Lastname";
            _customerObjecResponse = Repository.AddCustomer(Customer);
            Thread.Sleep(2000);
        };

        private Because Of = () =>
        {

            CustomerObject.Id = CustomerObjectId;
            CustomerObject.Name = "Sture";
            CustomerObject.ResponsibleGuardFirstname = "ResponsibleGuardFirstname";
            CustomerObject.ResponsibleGuardLastname = "ResponsibleGuardLastname";
            CustomerObject.ResponsibleManagerFistname = "ResponsibleManagerFistname";
            CustomerObject.ResponsibleManagerLastname = "ResponsibleManagerLastname";
            CustomerObject.HourlyRate = 200;
            CustomerObject.License = true;
            CustomerObject.LicenseType = "LicenseType";
            CustomerObject.CompanyId = CompanyId;
            CustomerObject.CustomerId = CustomerId;
            _customerObjecResponse = Repository.AddObjectToCustomer(CustomerObject);
            _savedCustomerObject = Repository.GetCustomerObjectById(CustomerObject);
            Need.Id = NeedId;
            Need.NumberOfPersonalNeeded = "5";
            Need.StartDateTime = _startDateTime;
            Need.EndDateTime = _endDateTime;
            Need.CustomerObjectId = CustomerObjectId;
            Need.CustomerId = CustomerId;
            Need.CompanyId = CompanyId;
            _needResponse = Repository.AddNeedToCustomerObject(Need);
            _savedNeed = Repository.GetNeedFromCustomerObjectById(Need);
            _savedNeed.StartDateTime = _newStartDateTime;
            _savedNeed.EndDateTime = _newEndDateTime;
            _savedNeed.NumberOfPersonalNeeded = "7";
            _needResponse = Repository.ModifyNeedOnCustomerObject(_savedNeed);
            _modifiedNeed = Repository.GetNeedFromCustomerObjectById(_savedNeed);
        };

        private It should_modify_a_need_on_a_customer_object = () =>
        {
            _savedCustomerObject.Id.ShouldEqual(CustomerObjectId);
            _savedCustomerObject.Name.ShouldEqual("Sture");
            _savedCustomerObject.ResponsibleGuardFirstname.ShouldEqual("ResponsibleGuardFirstname");
            _savedCustomerObject.ResponsibleGuardLastname.ShouldEqual("ResponsibleGuardLastname");
            _savedCustomerObject.ResponsibleManagerFistname.ShouldEqual("ResponsibleManagerFistname");
            _savedCustomerObject.ResponsibleManagerLastname.ShouldEqual("ResponsibleManagerLastname");
            _savedCustomerObject.HourlyRate = 200;
            _savedCustomerObject.License = true;
            _savedCustomerObject.LicenseType = "LicenseType";
            _modifiedNeed.Id.ShouldEqual(NeedId);
            _modifiedNeed.NumberOfPersonalNeeded.ShouldEqual("7");
            _modifiedNeed.StartDateTime.ShouldEqual(_newStartDateTime);
            _modifiedNeed.EndDateTime.ShouldEqual(_newEndDateTime);
        };
    }
}

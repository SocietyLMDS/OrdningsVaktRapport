using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_Auto_2fReportEntities_2fByCompanyIdAndEmployeeId : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fReportEntities_2fByCompanyIdAndEmployeeId()
	{
		this.ViewText = @"from doc in docs.ReportEntities
select new { EmployeeId = doc.EmployeeId, CompanyId = doc.CompanyId }";
		this.ForEntityNames.Add("ReportEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "ReportEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				EmployeeId = doc.EmployeeId,
				CompanyId = doc.CompanyId,
				__document_id = doc.__document_id
			});
		this.AddField("EmployeeId");
		this.AddField("CompanyId");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("EmployeeId");
		this.AddQueryParameterForMap("CompanyId");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("EmployeeId");
		this.AddQueryParameterForReduce("CompanyId");
		this.AddQueryParameterForReduce("__document_id");
	}
}

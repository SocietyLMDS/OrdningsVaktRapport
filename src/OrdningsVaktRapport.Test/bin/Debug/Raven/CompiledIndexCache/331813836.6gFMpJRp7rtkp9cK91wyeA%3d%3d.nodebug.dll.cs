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


public class Index_Auto_2fCustomerEntities_2fByCompanyIdAndCustomerName : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fCustomerEntities_2fByCompanyIdAndCustomerName()
	{
		this.ViewText = @"from doc in docs.CustomerEntities
select new { CustomerName = doc.CustomerName, CompanyId = doc.CompanyId }";
		this.ForEntityNames.Add("CustomerEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "CustomerEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				CustomerName = doc.CustomerName,
				CompanyId = doc.CompanyId,
				__document_id = doc.__document_id
			});
		this.AddField("CustomerName");
		this.AddField("CompanyId");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("CustomerName");
		this.AddQueryParameterForMap("CompanyId");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("CustomerName");
		this.AddQueryParameterForReduce("CompanyId");
		this.AddQueryParameterForReduce("__document_id");
	}
}

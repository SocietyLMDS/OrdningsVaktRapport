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


public class Index_Auto_2fCustomerEntities_2fByCompanyIdAndName : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fCustomerEntities_2fByCompanyIdAndName()
	{
		this.ViewText = @"from doc in docs.CustomerEntities
select new { CompanyId = doc.CompanyId, Name = doc.Name }";
		this.ForEntityNames.Add("CustomerEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "CustomerEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				CompanyId = doc.CompanyId,
				Name = doc.Name,
				__document_id = doc.__document_id
			});
		this.AddField("CompanyId");
		this.AddField("Name");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("CompanyId");
		this.AddQueryParameterForMap("Name");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("CompanyId");
		this.AddQueryParameterForReduce("Name");
		this.AddQueryParameterForReduce("__document_id");
	}
}

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


public class Index_Auto_2fObjectEntities_2fByCompanyId : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fObjectEntities_2fByCompanyId()
	{
		this.ViewText = @"from doc in docs.ObjectEntities
select new { CompanyId = doc.CompanyId }";
		this.ForEntityNames.Add("ObjectEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "ObjectEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				CompanyId = doc.CompanyId,
				__document_id = doc.__document_id
			});
		this.AddField("CompanyId");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("CompanyId");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("CompanyId");
		this.AddQueryParameterForReduce("__document_id");
	}
}
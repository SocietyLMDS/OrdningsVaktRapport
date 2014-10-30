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


public class Index_Auto_2fCompanyEntities_2fByEmailAddress : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fCompanyEntities_2fByEmailAddress()
	{
		this.ViewText = @"from doc in docs.CompanyEntities
select new { EmailAddress = doc.EmailAddress }";
		this.ForEntityNames.Add("CompanyEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "CompanyEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				EmailAddress = doc.EmailAddress,
				__document_id = doc.__document_id
			});
		this.AddField("EmailAddress");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("EmailAddress");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("EmailAddress");
		this.AddQueryParameterForReduce("__document_id");
	}
}

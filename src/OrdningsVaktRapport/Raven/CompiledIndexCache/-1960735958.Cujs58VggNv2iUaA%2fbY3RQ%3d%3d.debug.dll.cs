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


public class Index_Auto_2fEmployeeEntities_2fByPasswordAndUsername : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fEmployeeEntities_2fByPasswordAndUsername()
	{
		this.ViewText = @"from doc in docs.EmployeeEntities
select new { Username = doc.Username, Password = doc.Password }";
		this.ForEntityNames.Add("EmployeeEntities");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "EmployeeEntities", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Username = doc.Username,
				Password = doc.Password,
				__document_id = doc.__document_id
			});
		this.AddField("Username");
		this.AddField("Password");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Username");
		this.AddQueryParameterForMap("Password");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Username");
		this.AddQueryParameterForReduce("Password");
		this.AddQueryParameterForReduce("__document_id");
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace OrdningsVaktRapport.Data.Utils
{
    class StoreFactory
    {
        public static DocumentStore CreateDocumentRavenDbStore()
        {
            //azure http://overout.cloudapp.net/ravendb
            //localt http://localhost/ravendb
            var ravenDbStore = new DocumentStore { Url = "http://localhost/ravendb" };
            ravenDbStore.Initialize();
            return ravenDbStore;
        }

        public static DocumentStore CreateInMemoryRavenDbStore()
        {
            var ravenDbStore = new EmbeddableDocumentStore { RunInMemory = true };
            ravenDbStore.Initialize();
            return ravenDbStore;
        }
    }
}

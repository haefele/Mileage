using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mileage.Shared.Entities.Search;
using Raven.Client.Indexes;
using Raven.Database.FileSystem.Storage.Esent.Schema.Updates;

namespace Mileage.Server.Infrastructure.Raven.Indexes
{
    public class DocumentsWithTags : AbstractMultiMapIndexCreationTask<DocumentsWithTags.Result>
    {
        public class Result
        {
            public string Tag { get; set; }
            public int Count { get; set; }
        }

        public DocumentsWithTags()
        {
            this.AddMapForAll<ITaggable>(documents => 
                from document in documents
                from tag in document.Tags
                select new
                {
                    Tag = tag,
                    Count = 1
                });

            this.Reduce = results =>
                from result in results
                group result by result.Tag into g
                select new
                {
                    Tag = g.Key,
                    Count = g.Sum(f => f.Count)
                };
        }

        public override string IndexName
        {
            get { return "Documents/WithTags"; }
        }
    }
}

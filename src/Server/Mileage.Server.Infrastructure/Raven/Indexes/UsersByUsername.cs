using System.Linq;
using Mileage.Shared.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Raven.Indexes
{
    public class UsersByUsername : AbstractIndexCreationTask<User>
    {
        public UsersByUsername()
        {
            this.Map = users => from user in users
                                select new
                                {
                                    user.Username
                                };

            this.Index(f => f.Username, FieldIndexing.NotAnalyzed);
        }

        public override string IndexName
        {
            get { return "Users/ByUsername"; }
        }
    }
}
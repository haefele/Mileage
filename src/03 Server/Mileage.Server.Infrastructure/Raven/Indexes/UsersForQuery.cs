using System.Linq;
using Mileage.Shared.Entities;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Raven.Indexes
{
    public class UsersForQuery : AbstractIndexCreationTask<User>
    {
        public UsersForQuery()
        {
            this.Map = users => from user in users
                                select new
                                {
                                    user.Username,
                                    user.NotificationEmailAddress
                                };

            this.Index(f => f.Username, FieldIndexing.NotAnalyzed);
            this.Index(f => f.NotificationEmailAddress, FieldIndexing.NotAnalyzed);
        }

        public override string IndexName
        {
            get { return "Users/ForQuery"; }
        }
    }
}
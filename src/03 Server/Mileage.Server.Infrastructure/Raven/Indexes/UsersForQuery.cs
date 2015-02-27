using System.Linq;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Users;
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
                                    user.EmailAddress
                                };

            this.Index(f => f.EmailAddress, FieldIndexing.NotAnalyzed);
        }

        public override string IndexName
        {
            get { return "Users/ForQuery"; }
        }
    }
}
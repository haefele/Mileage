using System.Collections.Generic;
using LiteGuard;
using Mileage.Shared.Models;

namespace Mileage.Server.Contracts.Commands.Search
{
    public class GetTopTagsWithCountsCommand : ICommand<IList<TagWithCount>>
    {
        public int CountOfTags { get; private set; }

        public GetTopTagsWithCountsCommand(int countOfTags)
        {
            this.CountOfTags = countOfTags;
        }
    }
}
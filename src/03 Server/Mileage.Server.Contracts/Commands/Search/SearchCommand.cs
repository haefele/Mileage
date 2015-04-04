using System.Collections.Generic;
using Mileage.Shared.Models;

namespace Mileage.Server.Contracts.Commands.Search
{
    public class SearchCommand : ICommand<SearchResult>
    {
        public string SearchText { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public SearchCommand(string searchText, int skip, int take)
        {
            this.SearchText = searchText;
            this.Skip = skip;
            this.Take = take;
        }
    }
}
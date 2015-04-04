using System.Collections.Generic;
using System.Linq;
using Mileage.Server.Contracts.Commands.Drivers;
using Mileage.Shared.Models;

namespace Mileage.Server.Contracts.Commands.Search
{
    public class SearchResult
    {
        public static SearchResult WithItems(IEnumerable<SearchItem> items)
        {
            return new SearchResult
            {
                Status = SearchResultStatus.Found,
                Items = items.ToList()
            };
        }

        public static SearchResult WithSuggestions(IList<string> suggestions)
        {
            return new SearchResult
            {
                Status = SearchResultStatus.Suggestions,
                Suggestions = suggestions
            };
        }

        public static SearchResult WithNoResults()
        {
            return new SearchResult
            {
                Status = SearchResultStatus.None
            };
        }

        private SearchResult()
        {
            
        }

        public SearchResultStatus Status { get; private set; }
        public IList<SearchItem> Items { get; private set; }
        public IList<string> Suggestions { get; private set; }
    }
}
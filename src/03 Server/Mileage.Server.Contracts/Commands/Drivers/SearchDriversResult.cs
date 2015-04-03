using System.Collections.Generic;
using Mileage.Shared.Entities.Drivers;

namespace Mileage.Server.Contracts.Commands.Drivers
{
    public class SearchDriversResult
    {
        public static SearchDriversResult WithDrivers(IList<Driver> drivers)
        {
            return new SearchDriversResult
            {
                Status = SearchDriversResultStatus.Found,
                FoundDrivers = drivers
            };
        }

        public static SearchDriversResult WithSuggestions(IList<string> suggestions)
        {
            return new SearchDriversResult
            {
                Status = SearchDriversResultStatus.Suggestions,
                Suggestions = suggestions
            };
        }

        public static SearchDriversResult WithNoResults()
        {
            return new SearchDriversResult
            {
                Status = SearchDriversResultStatus.None
            };
        }

        private SearchDriversResult()
        {
            
        }

        public SearchDriversResultStatus Status { get; private set; }
        public IList<Driver> FoundDrivers { get; private set; }
        public IList<string> Suggestions { get; private set; }
    }
}
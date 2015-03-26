using System.Collections.Generic;
using Mileage.Shared.Entities.Drivers;
using Raven.Abstractions.Data;

namespace Mileage.Server.Infrastructure.Commands.Drivers
{
    public class SearchDriversResult
    {
        public SearchDriversResultStatus Status { get; set; }
        public IList<Driver> FoundDrivers { get; set; }
        public IList<string> Suggestions { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Mileage.Shared.Entities.Search;

namespace Mileage.Shared.Entities.Vehicles
{
    public class Vehicle : AggregateRoot, ITaggable
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTimeOffset FirstRegistrationDate { get; set; }
        public List<string> Tags { get; set; }
    }
}
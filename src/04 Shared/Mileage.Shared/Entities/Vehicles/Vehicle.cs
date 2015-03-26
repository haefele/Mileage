using System;

namespace Mileage.Shared.Entities.Vehicles
{
    public class Vehicle : AggregateRoot
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTimeOffset FirstRegistrationDate { get; set; }
    }
}
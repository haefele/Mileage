using System;

namespace Mileage.Shared.Entities.Drivers
{
    public class DriversLicense
    {
        public string Class { get; set; }
        public DateTimeOffset Since { get; set; }
    }
}
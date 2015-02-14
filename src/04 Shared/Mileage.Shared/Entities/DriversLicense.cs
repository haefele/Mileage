using System;

namespace Mileage.Shared.Entities
{
    public class DriversLicense
    {
        public string Class { get; set; }
        public DateTimeOffset Since { get; set; }
    }
}
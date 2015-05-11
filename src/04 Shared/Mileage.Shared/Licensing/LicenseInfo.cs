using System;

namespace Mileage.Shared.Licensing
{
    public class LicenseInfo
    {
        public Guid Id { get; set; }

        public CustomerInfo Customer { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Version SupportedVersion { get; set; }
        public string[] SupportedProducts { get; set; }
    }
}
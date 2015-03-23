using System;
using System.Collections.Generic;
using Mileage.Shared.Configuration;

namespace Mileage.Server.Infrastructure
{
    public static class Config
    {
        static Config()
        {
            LicensePath = new StringSetting("Mileage/LicensePath", string.Empty);
            EnableDefaultMetrics = new BoolSetting("Mileage/EnableDefaultMetrics", false);
            CompressResponses = new BoolSetting("Mileage/CompressResponses", true);
            EnableDebugRequestResponseLogging = new BoolSetting("Mileage/EnableDebugRequestResponseLogging", false);
            FormatResponses = new BoolSetting("Mileage/FormatResponses", false);
            RavenHttpServerPort = new IntSetting("Mileage/RavenHttpServerPort", 8000);
            RavenName = new StringSetting("Mileage/RavenName", "Mileage");
            EnableRavenHttpServer = new BoolSetting("Mileage/EnableRavenHttpServer", false);
            Addresses = new UriListSetting("Mileage/Addresses", new List<Uri>() { new Uri("http://localhost") }, "|");
        }

        public static StringSetting LicensePath { get; private set; }
        public static BoolSetting EnableDefaultMetrics { get; private set; }
        public static BoolSetting CompressResponses { get; private set; }
        public static BoolSetting EnableDebugRequestResponseLogging { get; private set; }
        public static BoolSetting FormatResponses { get; private set; }
        public static IntSetting RavenHttpServerPort { get; private set; }
        public static StringSetting RavenName { get; private set; }
        public static BoolSetting EnableRavenHttpServer { get; private set; }
        public static UriListSetting Addresses { get; private set; }
    }
}
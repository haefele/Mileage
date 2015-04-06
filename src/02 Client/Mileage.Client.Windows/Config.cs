using System;
using Mileage.Shared.Configuration;

namespace Mileage.Client.Windows
{
    public static class Config
    {
        static Config()
        {
            EmbeddedDatabaseName = new StringSetting("Mileage/EmbeddedDatabaseName", "./Database.db");
            WebServiceAddress = new StringSetting("Mileage/WebServiceAddress", string.Empty);
            KeyDownDelay = new TimeSpanSetting("Mileage/KeyDownDelay", TimeSpan.FromMilliseconds(300));
        }

        public static StringSetting EmbeddedDatabaseName { get; private set; }
        public static StringSetting WebServiceAddress { get; private set; }
        public static TimeSpanSetting KeyDownDelay { get; private set; }
    }
}
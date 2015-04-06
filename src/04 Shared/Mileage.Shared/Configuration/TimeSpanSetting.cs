using System;
using System.Configuration;
using JetBrains.Annotations;

namespace Mileage.Shared.Configuration
{
    public class TimeSpanSetting : BaseSetting<TimeSpan>
    {
        public TimeSpanSetting([NotNull] string appSettingsKey, [NotNull] TimeSpan defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        protected override bool TryParse(string stringValue, out TimeSpan value)
        {
            return TimeSpan.TryParse(stringValue, out value);
        }
    }
}
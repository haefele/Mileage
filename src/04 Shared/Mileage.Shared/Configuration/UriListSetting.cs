using System;
using System.Collections.Generic;
using System.Linq;

namespace Mileage.Shared.Configuration
{
    public class UriListSetting : BaseSetting<List<Uri>>
    {
        private readonly string _separator;

        public UriListSetting(string appSettingsKey, List<Uri> defaultValue, string separator)
            : base(appSettingsKey, defaultValue)
        {
            this._separator = separator;
        }

        protected override bool TryParse(string stringValue, out List<Uri> value)
        {
            string[] parts = stringValue.Split(new[] {this._separator}, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Uri>();

            foreach (string part in parts)
            {
                Uri uri;
                if (Uri.TryCreate(part, UriKind.Absolute, out uri))
                {
                    result.Add(uri);
                }
            }

            value = result;

            return result.Any();
        }
    }
}
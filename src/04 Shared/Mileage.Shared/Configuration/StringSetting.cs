namespace Mileage.Shared.Configuration
{
    public class StringSetting : BaseSetting<string>
    {
        public StringSetting(string appSettingsKey, string defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        protected override bool TryParse(string stringValue, out string value)
        {
            value = stringValue;

            return true;
        }
    }
}
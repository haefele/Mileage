namespace Mileage.Shared.Configuration
{
    public class BoolSetting : BaseSetting<bool>
    {
        public BoolSetting(string appSettingsKey, bool defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        protected override bool TryParse(string stringValue, out bool value)
        {
            return bool.TryParse(stringValue, out value);
        }
    }
}
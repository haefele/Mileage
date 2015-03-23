namespace Mileage.Shared.Configuration
{
    public class IntSetting : BaseSetting<int>
    {
        public IntSetting(string appSettingsKey, int defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        protected override bool TryParse(string stringValue, out int value)
        {
            return int.TryParse(stringValue, out value);
        }
    }
}
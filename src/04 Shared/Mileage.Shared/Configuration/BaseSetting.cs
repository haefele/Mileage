using System.Configuration;

namespace Mileage.Shared.Configuration
{
    public abstract class BaseSetting<T>
    {
        private readonly string _appSettingsKey;
        private readonly T _defaultValue;

        protected BaseSetting(string appSettingsKey, T defaultValue)
        {
            this._appSettingsKey = appSettingsKey;
            this._defaultValue = defaultValue;
        }

        protected abstract bool TryParse(string stringValue, out T value);

        public T GetValue()
        {
            string stringValue = ConfigurationManager.AppSettings.Get(this._appSettingsKey) ?? string.Empty;

            T value;
            if (this.TryParse(stringValue, out value))
                return value;

            return this._defaultValue;
        }
    }
}
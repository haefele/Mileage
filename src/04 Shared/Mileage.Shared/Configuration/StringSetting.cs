﻿namespace Mileage.Shared.Configuration
{
    /// <summary>
    /// A <see cref="string"/> setting.
    /// </summary>
    public class StringSetting : BaseSetting<string>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSetting"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">The default value.</param>
        public StringSetting(string appSettingsKey, string defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries the parse the specified <paramref name="stringValue" />. Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        protected override bool TryParse(string stringValue, out string value)
        {
            value = stringValue;

            return true;
        }
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Mileage.Client.Contracts.Localization
{
    public interface ILocalizationManager
    {
        /// <summary>
        /// Gets the current language.
        /// </summary>
        CultureInfo CurrentLanguage { get; }

        /// <summary>
        /// Changes the application language to the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="culture">The culture.</param>
        void ChangeLanguage(CultureInfo culture);
        /// <summary>
        /// Returns all supported languages.
        /// </summary>
        IEnumerable<CultureInfo> GetSupportedLanguages();

        /// <summary>
        /// Adds the language dependent action.
        /// </summary>
        /// <param name="action">The action.</param>
        void AddLanguageDependentAction(Action action);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;

namespace Mileage.Client.Contracts.Localization
{
    public interface ILocalizationManager
    {
        /// <summary>
        /// Gets the current language.
        /// </summary>
        [NotNull]
        CultureInfo CurrentLanguage { get; }
        /// <summary>
        /// Gets the current language changes.
        /// </summary>
        [NotNull]
        IObservable<CultureInfo> CurrentLanguageObservable { get; } 

        /// <summary>
        /// Changes the application language to the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="culture">The culture.</param>
        void ChangeLanguage([NotNull]CultureInfo culture);
        /// <summary>
        /// Returns all supported languages.
        /// </summary>
        [NotNull]
        IEnumerable<CultureInfo> GetSupportedLanguages();
    }
}
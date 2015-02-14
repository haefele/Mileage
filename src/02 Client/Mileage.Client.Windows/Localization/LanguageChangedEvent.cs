using System.Globalization;
using LiteGuard;

namespace Mileage.Client.Windows.Localization
{
    public class LanguageChangedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageChangedEvent"/> class.
        /// </summary>
        /// <param name="newLanguage">The new language.</param>
        public LanguageChangedEvent(CultureInfo newLanguage)
        {
            Guard.AgainstNullArgument("newLanguage", newLanguage);

            this.NewLanguage = newLanguage;
        }

        /// <summary>
        /// Gets the new language.
        /// </summary>
        public CultureInfo NewLanguage { get; private set; }
    }
}
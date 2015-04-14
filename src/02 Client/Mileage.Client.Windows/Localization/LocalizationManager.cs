using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Subjects;
using System.Threading;
using Anotar.NLog;
using Caliburn.Micro;
using LiteGuard;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Contracts.Storage;
using Mileage.Localization;
using WPFLocalizeExtension.Engine;

namespace Mileage.Client.Windows.Localization
{
    public class LocalizationManager : ILocalizationManager, IDisposable
    {
        #region Constants
        /// <summary>
        /// The key used to identify the current language.
        /// </summary>
        private const string CurrentLanguageId = "Mileage/CurrentLanguage";
        #endregion

        #region Fields
        private readonly IDataStorage _dataStorage;
        private readonly IEventAggregator _eventAggregator;

        private readonly ReplaySubject<CultureInfo> _currentLanguageObservable;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current language.
        /// </summary>
        public CultureInfo CurrentLanguage { get; private set; }
        /// <summary>
        /// Gets the current language changes.
        /// </summary>
        public IObservable<CultureInfo> CurrentLanguageObservable
        {
            get { return this._currentLanguageObservable; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationManager"/> class.
        /// </summary>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public LocalizationManager(IDataStorage dataStorage, IEventAggregator eventAggregator)
        {
            Guard.AgainstNullArgument("dataStorage", dataStorage);
            Guard.AgainstNullArgument("eventAggregator", eventAggregator);
            
            this._dataStorage = dataStorage;
            this._eventAggregator = eventAggregator;

            this._currentLanguageObservable = new ReplaySubject<CultureInfo>(1);

            this.LoadCurrentLanguage();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Changes the application language to the specified <paramref name="culture" />.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public void ChangeLanguage(CultureInfo culture)
        {
            Guard.AgainstNullArgument("culture", culture);

            if (this.CurrentLanguage.Equals(culture))
                return;

            this.ChangeLanguageInternal(culture);
        }
        /// <summary>
        /// Returns all supported languages.
        /// </summary>
        public IEnumerable<CultureInfo> GetSupportedLanguages()
        {
            return Languages.GetSupportedLanguages();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Internal implementation used to change the current language.
        /// </summary>
        /// <param name="culture">The culture.</param>
        private void ChangeLanguageInternal(CultureInfo culture)
        {
            LogTo.Debug("Changing application language to '{0}'.", culture.Name);

            this.CurrentLanguage = culture;

            LocalizeDictionary.Instance.Culture = culture;
            Execute.OnUIThread(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            });

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            this._currentLanguageObservable.OnNext(culture);
            this._eventAggregator.PublishOnUIThreadAsync(new LanguageChangedEvent(this.CurrentLanguage));
        }
        /// <summary>
        /// Loads the current language.
        /// </summary>
        private void LoadCurrentLanguage()
        {
            var languageData = this._dataStorage.Get<LanguageData>(CurrentLanguageId);

            CultureInfo language = languageData != null ?
                new CultureInfo(languageData.Language) :
                Languages.GetDefaultLanguage();

            this.ChangeLanguageInternal(language);
        }
        /// <summary>
        /// Saves the language.
        /// </summary>
        private void SaveLanguage()
        {
            this._dataStorage.Store(CurrentLanguageId, new LanguageData { Language = this.CurrentLanguage.Name });
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.SaveLanguage();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Holds the actual language inside the data storage.
        /// </summary>
        public class LanguageData
        {
            public string Language { get; set; }
        }
        #endregion
    }
}
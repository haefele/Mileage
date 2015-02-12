using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using Castle.Core.Logging;
using LiteGuard;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Contracts.Storage;
using Weakly;
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

        private readonly ConcurrentDictionary<Guid, WeakAction> _languageDependentActions;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
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

            this.Logger = NullLogger.Instance;

            this._dataStorage = dataStorage;
            this._eventAggregator = eventAggregator;

            this._languageDependentActions = new ConcurrentDictionary<Guid, WeakAction>();

            this.LoadCurrentLanguage();
        }
        #endregion

        #region Implementation of ILocalizationManager
        /// <summary>
        /// Gets the current language.
        /// </summary>
        public CultureInfo CurrentLanguage { get; private set; }
        /// <summary>
        /// Changes the application language to the specified <paramref name="culture" />.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public void ChangeLanguage(CultureInfo culture)
        {
            Guard.AgainstNullArgument("culture", culture);

            if (this.CurrentLanguage != null && this.CurrentLanguage.Equals(culture))
                return;

            this.Logger.DebugFormat("Changing application language to '{0}'.", culture.Name);

            this.CurrentLanguage = culture;

            LocalizeDictionary.Instance.Culture = culture;
            Execute.OnUIThread(() => 
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            });

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            this._eventAggregator.PublishOnUIThreadAsync(new LanguageChangedEvent(this.CurrentLanguage));

            this.ExecuteLanguageDependentActions();
            this.RemoveDeadLanguageDependentActions();
        }
        /// <summary>
        /// Returns all supported languages.
        /// </summary>
        public IEnumerable<CultureInfo> GetSupportedLanguages()
        {
            yield return new CultureInfo("de-DE");
            yield return new CultureInfo("en-US");
        }
        /// <summary>
        /// Adds the language dependent action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddLanguageDependentAction(System.Action action)
        {
            action();

            this._languageDependentActions.GetOrAdd(Guid.NewGuid(), f => new WeakAction(action));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the current language.
        /// </summary>
        private void LoadCurrentLanguage()
        {
            var languageData = this._dataStorage.Get<LanguageData>(CurrentLanguageId);
            CultureInfo language = languageData != null ? 
                new CultureInfo(languageData.Language) : 
                this.GetSupportedLanguages().First();

            this.ChangeLanguage(language);
        }
        /// <summary>
        /// Saves the language.
        /// </summary>
        private void SaveLanguage()
        {
            this._dataStorage.Store(CurrentLanguageId, new LanguageData { Language = this.CurrentLanguage.Name });
        }
        /// <summary>
        /// Removes the dead language dependent actions.
        /// </summary>
        private void RemoveDeadLanguageDependentActions()
        {
            foreach (var action in this._languageDependentActions)
            {
                if (action.Value.IsAlive == false)
                {
                    WeakAction weakAction;
                    this._languageDependentActions.TryRemove(action.Key, out weakAction);
                }
            }
        }
        /// <summary>
        /// Executes the language dependent actions.
        /// </summary>
        private void ExecuteLanguageDependentActions()
        {
            foreach (var action in this._languageDependentActions)
            {
                action.Value.Invoke();
            }
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
        private class LanguageData
        {
            public string Language { get; set; }
        }
        #endregion
    }
}
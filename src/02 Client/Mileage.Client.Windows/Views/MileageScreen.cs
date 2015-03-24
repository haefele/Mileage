using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using Castle.Core.Logging;
using Castle.Windsor;
using LiteGuard;
using Mileage.Client.Contracts.Exceptions;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Contracts.Storage;
using Mileage.Client.Windows.WebServices;
using Mileage.Localization.Common;

namespace Mileage.Client.Windows.Views
{
    public abstract class MileageScreen : ReactiveScreen
    {
        #region Fields
        private readonly IWindsorContainer _container;
        #endregion

        #region Logger
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the message service.
        /// </summary>
        protected IMessageService MessageService { get; set; }
        /// <summary>
        /// Gets or sets the exception handler.
        /// </summary>
        protected IExceptionHandler ExceptionHandler { get; set; }
        /// <summary>
        /// Gets or sets the event aggregator.
        /// </summary>
        protected IEventAggregator EventAggregator { get; set; }
        /// <summary>
        /// Gets or sets the localization manager.
        /// </summary>
        protected ILocalizationManager LocalizationManager { get; set; }
        /// <summary>
        /// Gets or sets the data storage.
        /// </summary>
        protected IDataStorage DataStorage { get; set; }
        /// <summary>
        /// Gets or sets the web service client.
        /// </summary>
        protected WebServiceClient WebService { get; set; }
        /// <summary>
        /// Gets or sets the current session.
        /// </summary>
        protected Session Session { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MileageScreen"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected MileageScreen(IWindsorContainer container)
        {
            Guard.AgainstNullArgument("container", container);

            this._container = container;

            this.Logger = NullLogger.Instance;

            this.MessageService = container.Resolve<IMessageService>();
            this.ExceptionHandler = container.Resolve<IExceptionHandler>();
            this.EventAggregator = container.Resolve<IEventAggregator>();
            this.LocalizationManager = container.Resolve<ILocalizationManager>();
            this.DataStorage = container.Resolve<IDataStorage>();
            this.WebService = container.Resolve<WebServiceClient>();
            this.Session = container.Resolve<Session>();

            this.LocalizationManager.AddLanguageDependentAction(this.UpdateDisplayName);

            this.EventAggregator.Subscribe(this);
        }
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Creates the specified <typeparamref name="TViewModel"/>.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the viewmodel.</typeparam>
        protected virtual TViewModel CreateViewModel<TViewModel>() where TViewModel : ReactivePropertyChangedBase
        {
            return this._container.Resolve<TViewModel>();
        }
        /// <summary>
        /// Updates the display name.
        /// </summary>
        private void UpdateDisplayName()
        {
            this.DisplayName = this.GetDisplayName();
        }
        /// <summary>
        /// Returns the display name.
        /// </summary>
        protected virtual string GetDisplayName()
        {
            return CommonMessages.Mileage;
        }
        #endregion
    }
}
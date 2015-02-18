using Caliburn.Micro.ReactiveUI;
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
    public abstract class MileageReactiveScreen : ReactiveScreen
    {
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
        /// Initializes a new instance of the <see cref="MileageReactiveScreen"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected MileageReactiveScreen(IWindsorContainer container)
        {
            Guard.AgainstNullArgument("container", container);

            this.MessageService = container.Resolve<IMessageService>();
            this.ExceptionHandler = container.Resolve<IExceptionHandler>();
            this.LocalizationManager = container.Resolve<ILocalizationManager>();
            this.DataStorage = container.Resolve<IDataStorage>();
            this.WebService = container.Resolve<WebServiceClient>();
            this.Session = container.Resolve<Session>();

            this.LocalizationManager.AddLanguageDependentAction(this.UpdateDisplayName);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the display name.
        /// </summary>
        private void UpdateDisplayName()
        {
            this.DisplayName = CommonMessages.Mileage;
        }
        #endregion
    }
}
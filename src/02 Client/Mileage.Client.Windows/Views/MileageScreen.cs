using System;
using System.Reactive.Linq;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Client.Contracts.Exceptions;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Contracts.Storage;
using Mileage.Client.Windows.WebServices;
using Mileage.Localization.Common;
using ReactiveUI;

namespace Mileage.Client.Windows.Views
{
    public abstract class MileageScreen : ReactiveScreen, IDisposable
    {
        #region Fields
        private readonly IWindsorContainer _container;
        private readonly ObservableAsPropertyHelper<string> _displayNameHelper;
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

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (this._displayNameHelper == null)
                    return string.Empty;

                return this._displayNameHelper.Value;
            }
            set { /* Do nothing, we dont allow setting the display name */ }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MileageScreen"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected MileageScreen([NotNull]IWindsorContainer container)
        {
            Guard.AgainstNullArgument("container", container);

            this._container = container;

            this.MessageService = container.Resolve<IMessageService>();
            this.ExceptionHandler = container.Resolve<IExceptionHandler>();
            this.EventAggregator = container.Resolve<IEventAggregator>();
            this.LocalizationManager = container.Resolve<ILocalizationManager>();
            this.DataStorage = container.Resolve<IDataStorage>();
            this.WebService = container.Resolve<WebServiceClient>();
            this.Session = container.Resolve<Session>();
            
            IObservable<string> observable = this.GetDisplayNameObservable();
            observable.ToProperty(this, f => f.DisplayName, out this._displayNameHelper);
        }
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Creates the specified <typeparamref name="TViewModel"/>.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the viewmodel.</typeparam>
        [CanBeNull]
        protected virtual TViewModel CreateViewModel<TViewModel>() where TViewModel : ReactivePropertyChangedBase
        {
            return this._container.Resolve<TViewModel>();
        }
        /// <summary>
        /// Gets the display name observable.
        /// Override this member if the <see cref="DisplayName"/> depends on more properties than just the current language.
        /// For example: If we add a user input to it, you override this methods and combine the base observable and your new one.
        /// </summary>
        [NotNull]
        protected virtual IObservable<string> GetDisplayNameObservable()
        {
            return this.LocalizationManager.CurrentLanguageObservable
                .Select(_ => this.GetDisplayName());
        }
        /// <summary>
        /// Gets the display name.
        /// Override this member if the <see cref="DisplayName"/> only depends on the current language.
        /// Otherwise override the <see cref="GetDisplayNameObservable"/> method.
        /// </summary>
        protected virtual string GetDisplayName()
        {
            return CommonMessages.Mileage;
        }
        /// <summary>
        /// Returns the parent with the specified type <typeparamref name="T"/>.
        /// This works recursively.
        /// </summary>
        /// <typeparam name="T">The type of the parent ViewModel.</typeparam>
        [CanBeNull]
        protected T GetParent<T>()
            where T : class
        {
            var parent = this.Parent as MileageScreen;
            while (parent != null && parent is T == false)
            {
                parent = parent.Parent as MileageScreen;
            }

            return parent as T;
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this._displayNameHelper != null)
                this._displayNameHelper.Dispose();
        }
        #endregion
    }
}
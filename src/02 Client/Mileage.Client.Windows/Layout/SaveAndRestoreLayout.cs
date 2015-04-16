using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using DevExpress.Mvvm.UI.Interactivity;
using Mileage.Client.Contracts.Layout;
using Mileage.Client.Windows.Events;
using Mileage.Client.Windows.WebServices;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.Layout
{
    public class SaveAndRestoreLayout : Behavior<FrameworkElement>, IHandleWithTask<UserLoggingOutEvent>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the layout.
        /// </summary>
        public string LayoutName { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAndRestoreLayout"/> class.
        /// </summary>
        public SaveAndRestoreLayout()
        {
            var eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.Subscribe(this);
        }
        #endregion

        #region Overrides of Behavior
        /// <summary>
        /// Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.AssociatedObjectOnLoaded;
            this.AssociatedObject.Unloaded -= this.AssociatedObjectOnUnloaded;
        }
        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += this.AssociatedObjectOnLoaded;
            this.AssociatedObject.Unloaded += this.AssociatedObjectOnUnloaded;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executed when the <see cref="Behavior{T}.AssociatedObject"/> is unloaded
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await SaveLayoutAsync();
        }
        /// <summary>
        /// Executed when the <see cref="Behavior{T}.AssociatedObject"/> is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var layoutManager = IoC.Get<ILayoutManager>();
            await layoutManager.RestoreLayoutForCurrentUserAsync(this.LayoutName, this.AssociatedObject);
        }
        /// <summary>
        /// Saves the layout asynchronously1.
        /// </summary>
        private async Task SaveLayoutAsync()
        {
            var layoutManager = IoC.Get<ILayoutManager>();
            await layoutManager.SaveLayoutForCurrentUserAsync(this.LayoutName, this.AssociatedObject);
        }
        #endregion

        #region Implementation of IHandle<UserLoggingOutEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public Task Handle(UserLoggingOutEvent message)
        {
            return this.SaveLayoutAsync();
        }
        #endregion
    }
}
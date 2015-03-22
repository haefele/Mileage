using System;
using System.Windows;
using Caliburn.Micro;
using DevExpress.Mvvm.UI.Interactivity;
using Mileage.Client.Contracts.Layout;
using Mileage.Client.Windows.WebServices;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.Layout
{
    public class SaveAndRestoreLayout : Behavior<FrameworkElement>
    {
        #region Fields
        private readonly User _currentUser;
        #endregion

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
            this._currentUser = IoC.Get<Session>().CurrentUser;
        }
        #endregion

        #region Private Methods
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.AssociatedObjectOnLoaded;
            this.AssociatedObject.Unloaded -= this.AssociatedObjectOnUnloaded;
        }
        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += this.AssociatedObjectOnLoaded;
            this.AssociatedObject.Unloaded += this.AssociatedObjectOnUnloaded;
        }
        private async void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var layoutManager = IoC.Get<ILayoutManager>();
            await layoutManager.SaveLayoutAsync(this._currentUser, this.LayoutName, this.AssociatedObject);
        }
        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //var layoutManager = IoC.Get<ILayoutManager>();
            //await layoutManager.RestoreLayoutAsync(this._currentUser, this.LayoutName, this.AssociatedObject);
        }
        #endregion
    }
}
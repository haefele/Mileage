using System;
using Castle.Windsor;
using Mileage.Client.Windows.Views.Shell.Items;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell
{
    public class ShellViewModel : MileageReactiveConductor<IAmDisplayedInShell>.Collection.OneActive
    {
        #region Commands
        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        public ReactiveCommand<object> Logout { get; private set; }
        #endregion

        public ShellViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }

        private void CreateCommands()
        {
            this.Logout = ReactiveCommand.Create();
            this.Logout.Subscribe(_ => this.LogoutImpl());
            this.Logout.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
        }

        protected override void OnInitialize()
        {
            this.ActivateItem(this.CreateViewModel<DriversRootViewModel>());
        }

        private void LogoutImpl()
        {
            this.Session.Clear();
            this.TryClose(true);
        }
    }
}

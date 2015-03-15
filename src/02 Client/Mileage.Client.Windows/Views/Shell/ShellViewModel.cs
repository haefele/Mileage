using System;
using System.Linq;
using Castle.Windsor;
using Mileage.Client.Windows.Views.Shell.Items;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell
{
    public class ShellViewModel : MileageConductor<IAmDisplayedInShell>.Collection.OneActive
    {
        #region Commands
        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        public ReactiveCommand<object> Logout { get; private set; }
        public ReactiveCommand<object> ChangeContent { get; private set; }
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

            this.ChangeContent = ReactiveCommand.Create();
            this.ChangeContent.Subscribe(_ => this.ChangeContentImpl());
            this.ChangeContent.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
        }

        protected override void OnInitialize()
        {
            this.CreateRootItems();
        }

        private void ChangeContentImpl()
        {
            if (this.ActiveItem is DriversRootViewModel)
                this.ActivateItem(this.Items.Last());
            else
                this.ActivateItem(this.Items.First());
        }

        private void CreateRootItems()
        {
            this.Items.Add(this.CreateViewModel<DriversRootViewModel>());

            this.ActivateItem(this.Items.First());
        }

        private void LogoutImpl()
        {
            this.Session.Clear();
            this.TryClose(true);
        }
    }
}

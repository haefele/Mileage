﻿using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using Mileage.Client.Windows.Events;
using Mileage.Client.Windows.Views.Shell.Items;
using Mileage.Shared.Entities.Users;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell
{
    public class ShellViewModel : MileageConductor<IShellItem>.Collection.OneActive
    {
        #region Commands
        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        public ReactiveCommand<object> Logout { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="shellItems">The shell items.</param>
        public ShellViewModel(IWindsorContainer container, IEnumerable<IShellItem> shellItems)
            : base(container)
        {
            this.CreateCommands();

            this.Items.AddRange(shellItems);
        }
        #endregion

        #region Private Methods
        protected override void OnInitialize()
        {
            this.ActivateItem(this.Items.First());
        }
        private void CreateCommands()
        {
            this.Logout = ReactiveCommand.Create();
            this.Logout.Subscribe(_ => this.LogoutImpl());
            this.Logout.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
        }
        private void LogoutImpl()
        {
            User user = this.Session.CurrentUser;
            this.EventAggregator.PublishOnCurrentThread(new UserLoggingOutEvent(user));
            this.Session.Clear();
            this.EventAggregator.PublishOnCurrentThread(new UserLoggedOutEvent(user));

            this.TryClose(true);
        }
        #endregion
    }
}

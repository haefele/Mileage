﻿using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using Castle.Windsor;
using Mileage.Localization.Client.Shell;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items
{
    public class DriversRootViewModel : ShellItemViewModel
    {
        public ReactiveCommand<object> DoSomething { get; private set; }

        public DriversRootViewModel(IWindsorContainer container) 
            : base(container)
        {
            this.DoSomething = ReactiveCommand.Create();
            this.DoSomething.Subscribe(_ =>
            {
                MessageBox.Show("asdf");
            });

            this.ActivateItem(new RoutesRootViewModel(container));
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Drivers;
        }
    }
}
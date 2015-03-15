using System;
using System.Windows;
using Castle.Windsor;
using Mileage.Localization.Client.Shell;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Drivers
{
    public class DriversRootViewModel : MileageScreen, IShellItem
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
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Drivers;
        }

        public MileageScreen PopupViewModel { get; private set; }
    }
}
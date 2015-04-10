using System;
using System.Linq;
using System.Reactive.Linq;
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
                this.LocalizationManager.ChangeLanguage(this.LocalizationManager.GetSupportedLanguages().FirstOrDefault(f => f.Equals(this.LocalizationManager.CurrentLanguage) == false)));
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Drivers;
        }

        public MileageScreen PopupViewModel { get; private set; }
    }
}
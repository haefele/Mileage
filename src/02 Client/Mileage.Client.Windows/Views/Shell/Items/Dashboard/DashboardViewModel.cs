using System;
using System.Collections;
using System.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using Mileage.Client.Windows.Extensions;
using Mileage.Client.Windows.Views.SimpleSearch;
using Mileage.Client.Windows.Views.TagCloud;
using Mileage.Localization.Client.Shell;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class DashboardViewModel : MileageConductor<DashboardItemViewModel>.Collection.AllActive, IShellItem
    {
        #region Commands
        public ReactiveCommand<object> ManageDashboardItems { get; private set; }
        #endregion

        public DashboardViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }

        private void CreateCommands()
        {
            this.ManageDashboardItems = ReactiveCommand.Create();
            this.ManageDashboardItems.Subscribe(_ =>
            {
                this.Items.Remove(this.Items.LastOrDefault());
            });
        }

        protected override void OnInitialize()
        {
            this.PopupViewModel = this.CreateViewModel<SimpleSearchViewModel>();
            this.PopupViewModel.TryActivate();

            this.Items.Add(this.CreateViewModel<DashboardItemViewModel>());
            this.Items[0].ActivateItem(this.CreateViewModel<SimpleSearchViewModel>());
            this.Items[0].DashboardItemName = "Erstes Panel";
            this.Items.Add(this.CreateViewModel<DashboardItemViewModel>());
            this.Items[1].ActivateItem(this.CreateViewModel<SimpleSearchViewModel>());
            this.Items[1].DashboardItemName = "Noch ein drittes Panel, omg!";
            this.Items.Add(this.CreateViewModel<DashboardItemViewModel>());
            this.Items[2].ActivateItem(this.CreateViewModel<TagCloudViewModel>());
            this.Items[2].DashboardItemName = "Zweites Panel!";
        }

        protected override void OnDeactivate(bool close)
        {
            //Only send the close deactivation to the popup view model
            //The PopupViewModel should not receive subsequent Activate and Deactivate lifetime calls
            if (close)
                this.PopupViewModel.TryDeactivate(true);
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Dashboard;
        }

        #region Implementation of IShellItem
        public MileageScreen PopupViewModel { get; private set; }
        #endregion
    }
}
using System;
using System.Collections;
using Caliburn.Micro;
using Castle.Windsor;
using Mileage.Client.Windows.Extensions;
using Mileage.Localization.Client.Shell;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class DashboardViewModel : MileageConductor<MileageScreen>.Collection.AllActive, IShellItem
    {
        public DashboardViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override void OnInitialize()
        {
            this.PopupViewModel = this.CreateViewModel<DashboardPopupViewModel>();
            this.PopupViewModel.TryActivate();
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
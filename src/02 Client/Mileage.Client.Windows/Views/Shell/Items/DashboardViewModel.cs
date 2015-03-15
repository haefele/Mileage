using System.Collections.Generic;
using System.Globalization;
using Castle.Windsor;
using Mileage.Localization.Client.Shell;

namespace Mileage.Client.Windows.Views.Shell.Items
{
    public class DashboardViewModel : ShellItemViewModel
    {
        public DashboardViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Dashboard;
        }
    }
}
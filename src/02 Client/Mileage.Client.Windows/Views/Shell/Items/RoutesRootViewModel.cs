using Castle.Windsor;
using Mileage.Localization.Client.Shell;

namespace Mileage.Client.Windows.Views.Shell.Items
{
    public class RoutesRootViewModel : ShellItemViewModel
    {
        public RoutesRootViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Routes;
        }
    }
}
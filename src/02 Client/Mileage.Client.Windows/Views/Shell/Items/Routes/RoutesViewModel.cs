using Castle.Windsor;
using Mileage.Localization.Client.Shell;

namespace Mileage.Client.Windows.Views.Shell.Items.Routes
{
    public class RoutesRootViewModel : MileageScreen, IShellItem
    {
        public RoutesRootViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override string GetDisplayName()
        {
            return ShellMessages.Routes;
        }

        public MileageScreen PopupViewModel { get; private set; }
    }
}
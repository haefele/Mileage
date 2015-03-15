using Castle.Windsor;

namespace Mileage.Client.Windows.Views.Shell
{
    public abstract class ShellItemViewModel : MileageConductor<MileageScreen>
    {
        protected ShellItemViewModel(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
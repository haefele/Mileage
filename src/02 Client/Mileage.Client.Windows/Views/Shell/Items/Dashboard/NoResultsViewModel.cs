using Castle.Windsor;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class NoResultsViewModel : MileageScreen
    {
        public NoResultsViewModel(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
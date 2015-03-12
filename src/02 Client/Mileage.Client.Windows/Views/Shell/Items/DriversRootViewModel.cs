using Castle.Windsor;

namespace Mileage.Client.Windows.Views.Shell.Items
{
    public class DriversRootViewModel : MileageReactiveScreen, IAmDisplayedInShell
    {
        public DriversRootViewModel(IWindsorContainer container) 
            : base(container)
        {
        }


    }
}
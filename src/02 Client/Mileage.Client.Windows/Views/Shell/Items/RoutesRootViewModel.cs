using Castle.Windsor;

namespace Mileage.Client.Windows.Views.Shell.Items
{
    public class RoutesRootViewModel : MileageScreen, IAmDisplayedInShell
    {
        public RoutesRootViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }

        protected override string GetDisplayName()
        {
            return "Strecken";
        }
    }
}
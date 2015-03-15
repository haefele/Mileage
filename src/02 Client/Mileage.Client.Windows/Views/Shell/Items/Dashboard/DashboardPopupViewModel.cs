using Castle.Windsor;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class DashboardPopupViewModel : MileageScreen
    {
        public DashboardPopupViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }
    }
}
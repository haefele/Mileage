using System;
using System.Reactive.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class DashboardItemViewModel : MileageConductor<MileageScreen>
    {
        private string _dashboardItemName;

        public string DashboardItemName
        {
            get { return this._dashboardItemName; }
            set { this.RaiseAndSetIfChanged(ref this._dashboardItemName, value); }
        }
        
        public DashboardItemViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IObservable<string> GetDisplayNameObservable()
        {
            var baseObservable = base.GetDisplayNameObservable();
            var dashboardItemNameObservable = this.WhenAnyValue(f => f.DashboardItemName);

            return baseObservable.CombineLatest(dashboardItemNameObservable,
                (_, dashboardItemName) => dashboardItemName);
        }
    }
}
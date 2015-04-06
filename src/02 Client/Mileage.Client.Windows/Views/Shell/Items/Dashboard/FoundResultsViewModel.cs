using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class FoundResultsViewModel : MileageScreen
    {
        private ReactiveObservableCollection<SearchItem> _items;

        public ReactiveObservableCollection<SearchItem> Items
        {
            get { return this._items; }
            set { this.RaiseAndSetIfChanged(ref this._items, value); }
        }

        public FoundResultsViewModel(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
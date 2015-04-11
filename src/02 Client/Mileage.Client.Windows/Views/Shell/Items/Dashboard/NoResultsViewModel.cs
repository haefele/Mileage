using System;
using System.Reactive.Linq;
using System.Windows.Media;
using Castle.Windsor;
using Mileage.Client.Windows.Resources;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class NoResultsViewModel : SearchResultViewModel
    {
        private string _searchText;

        public string SearchText
        {
            get { return this._searchText; }
            set { this.RaiseAndSetIfChanged(ref this._searchText, value); }
        }

        public NoResultsViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IObservable<string> GetDisplayNameObservable()
        {
            var baseObservable = base.GetDisplayNameObservable();
            var searchTextObservable = this.WhenAnyValue(f => f.SearchText);

            return baseObservable.CombineLatest(searchTextObservable,
                (_, searchText) => string.Format("Leider keine Ergebnisse für \"{0}\"", searchText));
        }

        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentEmpty; }
        }
    }
}
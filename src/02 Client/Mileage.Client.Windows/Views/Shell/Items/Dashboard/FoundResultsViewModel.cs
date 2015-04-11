using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Windows.Resources;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class FoundResultsViewModel : SearchResultViewModel
    {
        #region Fields
        private ReactiveObservableCollection<SearchItem> _items;
        private SearchItem _selectedItem;

        private string _foundThroughSuggestion;
        #endregion

        #region Properties
        public ReactiveObservableCollection<SearchItem> Items
        {
            get { return this._items; }
            set { this.RaiseAndSetIfChanged(ref this._items, value); }
        }

        public SearchItem SelectedItem
        {
            get { return this._selectedItem; }
            set { this.RaiseAndSetIfChanged(ref this._selectedItem, value);}
        }

        public string FoundThroughSuggestion
        {
            get { return this._foundThroughSuggestion; }
            set { this.RaiseAndSetIfChanged(ref this._foundThroughSuggestion, value); }
        }

        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentInspector; }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the show selected item.
        /// </summary>
        public ReactiveCommand<object> ShowSelectedItem { get; private set; }
        #endregion

        #region Constructors
        public FoundResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }
        #endregion

        #region Private Methods

        private void CreateCommands()
        {
            var canShowSelectedItem = this.WhenAnyValue(f => f.SelectedItem, (SearchItem searchItem) => searchItem != null);
            this.ShowSelectedItem = ReactiveCommand.Create(canShowSelectedItem);
            this.ShowSelectedItem.Subscribe(_ =>
            {
                this.MessageService.ShowDialog("Zeige " + this.SelectedItem.DisplayName, "Yay", MessageImage.Information, "OK");
            });
        }
        protected override IObservable<string> GetDisplayNameObservable()
        {
            var baseObservable = base.GetDisplayNameObservable();
            var suggestionObservable = this.WhenAnyValue(f => f.FoundThroughSuggestion);

            return baseObservable.CombineLatest(suggestionObservable,
                (_, suggestion) => string.IsNullOrWhiteSpace(suggestion)
                    ? "Ergebnisse"
                    : string.Format("Ergebnisse für \"{0}\"", suggestion));
        }
        #endregion
    }
}
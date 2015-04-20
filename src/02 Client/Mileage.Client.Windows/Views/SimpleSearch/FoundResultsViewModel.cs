using System;
using System.Reactive.Linq;
using System.Windows.Media;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Windows.Resources;
using Mileage.Localization.Client.SimpleSearch;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public class FoundResultsViewModel : SearchResultViewModel
    {
        #region Fields
        private ReactiveObservableCollection<SearchItem> _items;
        private SearchItem _selectedItem;

        private string _foundThroughSuggestion;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="SearchItem"/>s.
        /// </summary>
        public ReactiveObservableCollection<SearchItem> Items
        {
            get { return this._items; }
            set { this.RaiseAndSetIfChanged(ref this._items, value); }
        }
        /// <summary>
        /// Gets or sets the selected <see cref="SearchItem"/>.
        /// </summary>
        public SearchItem SelectedItem
        {
            get { return this._selectedItem; }
            set { this.RaiseAndSetIfChanged(ref this._selectedItem, value);}
        }
        /// <summary>
        /// Gets or sets the suggestion that lead to the <see cref="Items"/>.
        /// </summary>
        public string FoundThroughSuggestion
        {
            get { return this._foundThroughSuggestion; }
            set { this.RaiseAndSetIfChanged(ref this._foundThroughSuggestion, value); }
        }
        /// <summary>
        /// Gets the image.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="FoundResultsViewModel"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public FoundResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            var canShowSelectedItem = this.WhenAnyValue(f => f.SelectedItem, (SearchItem searchItem) => searchItem != null);
            this.ShowSelectedItem = ReactiveCommand.Create(canShowSelectedItem);
            this.ShowSelectedItem.Subscribe(_ =>
            {
                this.MessageService.ShowDialog("Zeige " + this.SelectedItem.DisplayName, "Yay", MessageImage.Information, "OK");
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the display name observable.
        /// Override this member if the <see cref="MileageScreen.DisplayName" /> depends on more properties than just the current language.
        /// For example: If we add a user input to it, you override this methods and combine the base observable and your new one.
        /// </summary>
        protected override IObservable<string> GetDisplayNameObservable()
        {
            var baseObservable = base.GetDisplayNameObservable();
            var suggestionObservable = this.WhenAnyValue(f => f.FoundThroughSuggestion);

            return baseObservable.CombineLatest(suggestionObservable,
                (_, suggestion) => string.IsNullOrWhiteSpace(suggestion)
                    ? SimpleSearchMessages.Results
                    : string.Format(SimpleSearchMessages.ResultsFor, suggestion));
        }
        #endregion
    }
}
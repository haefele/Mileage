using System;
using System.Windows.Media;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Client.Windows.Resources;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public class SuggestionResultsViewModel : SearchResultViewModel
    {
        #region Fields
        private ReactiveObservableCollection<string> _suggestions;
        private string _selectedSuggestion;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the suggestions.
        /// </summary>
        public ReactiveObservableCollection<string> Suggestions
        {
            get { return this._suggestions; }
            set { this.RaiseAndSetIfChanged(ref this._suggestions, value); }
        }
        /// <summary>
        /// Gets or sets the selected suggestion.
        /// </summary>
        public string SelectedSuggestion
        {
            get { return this._selectedSuggestion; }
            set { this.RaiseAndSetIfChanged(ref this._selectedSuggestion, value); }
        }
        /// <summary>
        /// Gets the image.
        /// </summary>
        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentInfo; }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Executes the search for the currently <see cref="SelectedSuggestion"/>.
        /// </summary>
        public ReactiveCommand<object> SearchSuggestion { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionResultsViewModel"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SuggestionResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            var canSearchSuggestion = this.WhenAnyValue(f => f.SelectedSuggestion, f => string.IsNullOrWhiteSpace(f) == false);

            this.SearchSuggestion = ReactiveCommand.Create(canSearchSuggestion);
            this.SearchSuggestion.Subscribe(async _ =>
            {
                var parent = this.GetParent<SimpleSearchViewModel>();

                if (parent == null)
                    return;

                parent.SearchText = this.SelectedSuggestion;
                await parent.Search.ExecuteAsyncTask();
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the display name.
        /// Override this member if the <see cref="MileageScreen.DisplayName" /> only depends on the current language.
        /// Otherwise override the <see cref="MileageScreen.GetDisplayNameObservable" /> method.
        /// </summary>
        /// <returns></returns>
        protected override string GetDisplayName()
        {
            return "Meinten Sie vielleicht";
        }
        #endregion
    }
}
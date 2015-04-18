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
        public ReactiveObservableCollection<string> Suggestions
        {
            get { return this._suggestions; }
            set { this.RaiseAndSetIfChanged(ref this._suggestions, value); }
        }
        public string SelectedSuggestion
        {
            get { return this._selectedSuggestion; }
            set { this.RaiseAndSetIfChanged(ref this._selectedSuggestion, value); }
        }
        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentInfo; }
        }
        #endregion

        #region Commands
        public ReactiveCommand<object> SearchSuggestion { get; private set; }
        #endregion

        #region Constructors
        public SuggestionResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }

        private void CreateCommands()
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

        protected override string GetDisplayName()
        {
            return "Meinten Sie vielleicht";
        }

        #endregion
    }
}
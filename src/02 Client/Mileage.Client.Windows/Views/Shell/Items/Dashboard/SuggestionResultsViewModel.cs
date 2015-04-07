using System;
using System.Windows.Media;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core.HandleDecorator;
using Mileage.Client.Windows.Resources;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
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
        public ReactiveCommand<object> SelectSuggestion { get; private set; }
        #endregion

        #region Constructors
        public SuggestionResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }

        private void CreateCommands()
        {
            var canSelectSuggestion = this.WhenAnyValue(f => f.SelectedSuggestion, f => string.IsNullOrWhiteSpace(f) == false);
            this.SelectSuggestion = ReactiveCommand.Create(canSelectSuggestion);
            this.SelectSuggestion.Subscribe(async _ =>
            {
                var parent = this.GetParent<DashboardPopupViewModel>();

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
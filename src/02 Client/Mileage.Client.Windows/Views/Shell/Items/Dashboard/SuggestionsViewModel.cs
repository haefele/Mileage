using System;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core.HandleDecorator;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class SuggestionsViewModel : MileageScreen
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
        #endregion

        #region Commands
        public ReactiveCommand<object> SelectSuggestion { get; private set; }
        #endregion

        #region Constructors
        public SuggestionsViewModel(IWindsorContainer container)
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

        #endregion
    }
}
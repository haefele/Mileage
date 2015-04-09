using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Client.Windows.Resources;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class FoundResultsViewModel : SearchResultViewModel
    {
        #region Fields
        private ReactiveObservableCollection<SearchItem> _items;
        private string _foundThroughSuggestion;

        private string _actualDisplayName;
        #endregion

        public ReactiveObservableCollection<SearchItem> Items
        {
            get { return this._items; }
            set { this.RaiseAndSetIfChanged(ref this._items, value); }
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

        public FoundResultsViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateRules();
        }

        protected override string GetDisplayName()
        {
            return this._actualDisplayName;
        }
        
        private void CreateRules()
        {
            this.WhenAnyValue(f => f.FoundThroughSuggestion)
                .Subscribe(suggestion =>
                {
                    this._actualDisplayName = string.IsNullOrWhiteSpace(suggestion)
                        ? "Ergebnisse"
                        : string.Format("Ergebnisse für {0}", suggestion);

                    this.UpdateDisplayName();
                });
        }
    }
}
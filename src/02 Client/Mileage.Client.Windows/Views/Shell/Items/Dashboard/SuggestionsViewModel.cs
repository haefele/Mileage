using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class SuggestionsViewModel : MileageScreen
    {
        private ReactiveObservableCollection<string> _suggestions;

        public ReactiveObservableCollection<string> Suggestions
        {
            get { return this._suggestions; }
            set { this.RaiseAndSetIfChanged(ref this._suggestions, value); }
        }

        public SuggestionsViewModel(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
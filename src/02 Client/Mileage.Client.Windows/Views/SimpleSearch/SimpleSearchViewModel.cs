using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public class SimpleSearchViewModel : MileageConductor<SearchResultViewModel>
    {
        #region Fields
        private string _searchText;
        private ObservableAsPropertyHelper<bool> _isSearchingHelper; 
        #endregion

        #region Properties
        public string SearchText
        {
            get { return this._searchText; }
            set { this.RaiseAndSetIfChanged(ref this._searchText, value); }
        }
        public bool IsSearching
        {
            get { return this._isSearchingHelper.Value; }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit> Search { get; private set; }
        #endregion

        public SimpleSearchViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();
        }

        private void CreateCommands()
        {
            var can = this.WhenAnyValue(f => f.SearchText, f => string.IsNullOrWhiteSpace(f) == false);

            this.Search = ReactiveCommand.CreateAsyncTask(_ => this.SearchImpl());
            this.Search.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
            this.Search.IsExecuting
                .ToProperty(this, f => f.IsSearching, out this._isSearchingHelper);

            this.WhenAnyValue(f => f.SearchText)
                .Throttle(Config.KeyDownDelay, RxApp.MainThreadScheduler)
                .DistinctUntilChanged()
                .InvokeCommand(this, f => f.Search);
        }

        private async Task<Unit> SearchImpl()
        {
            if (string.IsNullOrWhiteSpace(this.SearchText))
            {
                this.ActivateItem(null);
                return Unit.Default;
            }

            HttpResponseMessage result = await this.WebService.SearchClient.SearchAsync(this.SearchText);

            switch (result.StatusCode)
            {
                case HttpStatusCode.Found:
                {

                    var viewModel = this.CreateViewModel<FoundResultsViewModel>();
                    viewModel.FoundThroughSuggestion = result.Headers.Contains("Through-Suggestion")
                        ? result.Headers.GetValues("Through-Suggestion").First()
                        : string.Empty;

                    viewModel.Items = await result.Content.ReadAsAsync<ReactiveObservableCollection<SearchItem>>();

                    this.ActivateItem(viewModel);

                    break;

                }
                case HttpStatusCode.SeeOther:
                {
                    var viewModel = this.CreateViewModel<SuggestionResultsViewModel>();
                    viewModel.Suggestions = await result.Content.ReadAsAsync<ReactiveObservableCollection<string>>();

                    this.ActivateItem(viewModel);

                    break;
                }
                case HttpStatusCode.NotFound:
                {
                    var viewModel = this.CreateViewModel<NoResultsViewModel>();
                    viewModel.SearchText = this.SearchText;

                    this.ActivateItem(viewModel);

                    break;
                }
                default:
                {
                    HttpError error = await result.Content.ReadAsAsync<HttpError>();
                    this.ExceptionHandler.Handle(error);

                    break;
                }
            }

            return Unit.Default;
        }
    }
}
using System;
using System.Reactive.Linq;
using System.Windows.Media;
using Castle.Windsor;
using Mileage.Client.Windows.Resources;
using Mileage.Localization.Client.SimpleSearch;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public class NoResultsViewModel : SearchResultViewModel
    {
        #region Fields
        private string _searchText;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the search text that resulted in no search results.
        /// </summary>
        public string SearchText
        {
            get { return this._searchText; }
            set { this.RaiseAndSetIfChanged(ref this._searchText, value); }
        }
        /// <summary>
        /// Gets the image.
        /// </summary>
        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentEmpty; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NoResultsViewModel"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public NoResultsViewModel(IWindsorContainer container)
            : base(container)
        {
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the display name observable.
        /// Override this member if the <see cref="MileageScreen.DisplayName" /> depends on more properties than just the current language.
        /// For example: If we add a user input to it, you override this methods and combine the base observable and your new one.
        /// </summary>
        /// <returns></returns>
        protected override IObservable<string> GetDisplayNameObservable()
        {
            var baseObservable = base.GetDisplayNameObservable();
            var searchTextObservable = this.WhenAnyValue(f => f.SearchText);

            return baseObservable.CombineLatest(searchTextObservable,
                (_, searchText) => string.Format(SimpleSearchMessages.NoResultsFor, searchText));
        }
        #endregion
    }
}
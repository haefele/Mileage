using System.Windows.Media;
using Castle.Windsor;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public abstract class SearchResultViewModel : MileageScreen
    {
        #region Properties
        /// <summary>
        /// Gets the image.
        /// </summary>
        public abstract ImageSource Image { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultViewModel"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected SearchResultViewModel(IWindsorContainer container)
            : base(container)
        {
        }
        #endregion
    }
}
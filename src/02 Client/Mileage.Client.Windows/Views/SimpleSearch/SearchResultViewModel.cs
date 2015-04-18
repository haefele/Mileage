using System.Windows.Media;
using Castle.Windsor;

namespace Mileage.Client.Windows.Views.SimpleSearch
{
    public abstract class SearchResultViewModel : MileageScreen
    {
        protected SearchResultViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        public abstract ImageSource Image { get; }
    }
}
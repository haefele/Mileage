using System.Windows.Media;
using Castle.Windsor;
using Mileage.Client.Windows.Resources;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    public class NoResultsViewModel : SearchResultViewModel
    {
        public NoResultsViewModel(IWindsorContainer container)
            : base(container)
        {
        }

        public override ImageSource Image
        {
            get { return Resource.Icon.DocumentEmpty; }
        }

        protected override string GetDisplayName()
        {
            return "Leider keine Ergebnisse";
        }
    }
}
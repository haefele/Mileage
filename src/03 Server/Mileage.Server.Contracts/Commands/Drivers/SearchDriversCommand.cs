using JetBrains.Annotations;

namespace Mileage.Server.Contracts.Commands.Drivers
{
    public class SearchDriversCommand : ICommand<SearchDriversResult>
    {
        public SearchDriversCommand([CanBeNull]string searchText, [CanBeNull]string driversLicense, [CanBeNull]bool? personCarriageLicense, int skip, int take)
        {
            this.SearchText = searchText;
            this.DriversLicense = driversLicense;
            this.PersonCarriageLicense = personCarriageLicense;
            this.Skip = skip;
            this.Take = take;
        }

        public string SearchText { get; private set; }
        public string DriversLicense { get; private set; }
        public bool? PersonCarriageLicense { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
    }
}
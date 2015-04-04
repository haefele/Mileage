using JetBrains.Annotations;

namespace Mileage.Server.Contracts.Commands.Drivers
{
    public class SearchDriversCommand : ICommand<SearchDriversResult>
    {
        public SearchDriversCommand([CanBeNull]string searchText, int skip, int take)
        {
            this.SearchText = searchText;
            this.Skip = skip;
            this.Take = take;
        }

        public string SearchText { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
    }
}
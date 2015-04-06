using LiteGuard;
using Mileage.Client.Contracts.WebServices;
using Mileage.Shared.Entities;

namespace Mileage.Client.Windows.WebServices
{
    public class WebServiceClient
    {
        public IAuthenticationClient Authentication { get; private set; }
        public IUsersClient Users { get; private set; }
        public ILayoutClient LayoutClient { get; private set; }
        public ISearchClient SearchClient { get; private set; }

        public WebServiceClient(IAuthenticationClient authenticationClient, IUsersClient usersClient, ILayoutClient layoutClient, ISearchClient searchClient)
        {
            Guard.AgainstNullArgument("authenticationClient", authenticationClient);
            Guard.AgainstNullArgument("usersClient", usersClient);
            Guard.AgainstNullArgument("layoutClient", layoutClient);
            Guard.AgainstNullArgument("searchClient", searchClient);

            this.Authentication = authenticationClient;
            this.Users = usersClient;
            this.LayoutClient = layoutClient;
            this.SearchClient = searchClient;
        }
    }
}
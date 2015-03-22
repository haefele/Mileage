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

        public WebServiceClient(IAuthenticationClient authenticationClient, IUsersClient users, ILayoutClient layoutClient)
        {
            this.Authentication = authenticationClient;
            this.Users = users;
            this.LayoutClient = layoutClient;
        }
    }
}
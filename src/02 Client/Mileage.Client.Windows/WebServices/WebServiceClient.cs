using LiteGuard;
using Mileage.Client.Contracts.WebServices;
using Mileage.Shared.Entities;

namespace Mileage.Client.Windows.WebServices
{
    public class WebServiceClient
    {
        public IAuthenticationClient Authentication { get; private set; }

        public WebServiceClient(IAuthenticationClient authenticationClient)
        {
            this.Authentication = authenticationClient;
        }
    }
}
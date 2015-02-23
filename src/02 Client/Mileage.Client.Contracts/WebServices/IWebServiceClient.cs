using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;

namespace Mileage.Client.Contracts.WebServices
{
    public interface IWebServiceClient
    {
        void SetAuthentication(AuthenticationToken token);

        IAuthenticationClient Authentication { get; }
    }
}
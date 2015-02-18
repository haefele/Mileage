using Mileage.Shared.Entities;

namespace Mileage.Client.Contracts.WebServices
{
    public interface IWebServiceClient
    {
        void SetAuthentication(AuthenticationToken token);

        IAuthenticationClient Authentication { get; }
    }
}
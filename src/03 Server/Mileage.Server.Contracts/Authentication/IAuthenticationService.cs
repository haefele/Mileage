using Microsoft.Owin;
using Mileage.Shared.Results;

namespace Mileage.Server.Contracts.Authentication
{
    public interface IAuthenticationService : IService
    {
        Result<string> GetAuthenticatedUserId(IOwinContext requestContext);
    }
}
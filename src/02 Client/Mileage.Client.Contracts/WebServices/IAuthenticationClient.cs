using System.Net.Http;
using System.Threading.Tasks;
using Mileage.Shared.Models;

namespace Mileage.Client.Contracts.WebServices
{
    public interface IAuthenticationClient
    {
        Task<HttpResponseMessage> LoginAsync(LoginData loginData);

        Task<HttpResponseMessage> RegisterAsync(CreateAdminUserData registerData);
    }
}
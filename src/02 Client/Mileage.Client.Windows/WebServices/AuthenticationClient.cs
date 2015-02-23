using System;
using System.Net.Http;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Client.Contracts.WebServices;
using Mileage.Shared.Models;

namespace Mileage.Client.Windows.WebServices
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly MileageClient _client;

        public AuthenticationClient(MileageClient client)
        {
            Guard.AgainstNullArgument("client", client);

            this._client = client;
        }

        public Task<HttpResponseMessage> LoginAsync(LoginData loginData)
        {
            var request = this._client.CreateRequest("Authentication/Login", HttpMethod.Post, loginData);
            return this._client.SendRequestAsync(request);
        }

        public Task<HttpResponseMessage> RegisterAsync(CreateAdminUserData registerData)
        {
            var request = this._client.CreateRequest("Admin/CreateAdminUser", HttpMethod.Post, registerData);
            return this._client.SendRequestAsync(request);
        }
    }
}
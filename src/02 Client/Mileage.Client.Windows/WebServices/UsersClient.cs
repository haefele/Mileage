using System.Net.Http;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Client.Contracts.WebServices;

namespace Mileage.Client.Windows.WebServices
{
    public class UsersClient : IUsersClient
    {
        private readonly MileageClient _client;

        public UsersClient(MileageClient client)
        {
            Guard.AgainstNullArgument("client", client);

            this._client = client;
        }

        public Task<HttpResponseMessage> GetMeAsync()
        {
            var request = this._client.CreateRequest("Users/Me", HttpMethod.Get);
            return this._client.SendRequestAsync(request);
        }
    }
}
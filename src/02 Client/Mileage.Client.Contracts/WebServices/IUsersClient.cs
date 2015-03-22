using System.Net.Http;
using System.Threading.Tasks;

namespace Mileage.Client.Contracts.WebServices
{
    public interface IUsersClient
    {
        Task<HttpResponseMessage> GetMe();
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace Mileage.Client.Contracts.WebServices
{
    public interface ISearchClient
    {
        Task<HttpResponseMessage> SearchAsync(string searchText, int skip = 0, int take = 50);
    }
}
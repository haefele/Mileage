using System.Net.Http;
using System.Threading.Tasks;
using Mileage.Shared.Entities.Layout;

namespace Mileage.Client.Contracts.WebServices
{
    public interface ILayoutClient
    {
        Task<HttpResponseMessage> SaveLayout(StoredLayout layout);

        Task<HttpResponseMessage> GetLayout(string layoutName);
    }
}
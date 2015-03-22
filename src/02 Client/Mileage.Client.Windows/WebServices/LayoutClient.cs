using System.Net.Http;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Client.Contracts.WebServices;
using Mileage.Shared.Entities.Layout;

namespace Mileage.Client.Windows.WebServices
{
    public class LayoutClient : ILayoutClient
    {
        private readonly MileageClient _mileageClient;

        public LayoutClient(MileageClient mileageClient)
        {
            Guard.AgainstNullArgument("mileageClient", mileageClient);

            this._mileageClient = mileageClient;
        }

        public Task<HttpResponseMessage> SaveLayout(StoredLayout layout)
        {
            var request = this._mileageClient.CreateRequest("Layout", HttpMethod.Post, layout);
            return this._mileageClient.SendRequestAsync(request);
        }

        public Task<HttpResponseMessage> GetLayout(string layoutName)
        {
            var queryBuilder = new HttpQueryBuilder();
            queryBuilder.AddParameter("layoutName", layoutName);

            var request = this._mileageClient.CreateRequest(string.Format("Layout{0}", queryBuilder), HttpMethod.Get);
            return this._mileageClient.SendRequestAsync(request);
        }
    }
}
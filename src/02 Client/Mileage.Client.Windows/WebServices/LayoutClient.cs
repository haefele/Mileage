using System.Collections.Generic;
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

        public Task<HttpResponseMessage> SaveLayoutAsync(string layoutName, Dictionary<string, byte[]> layoutData)
        {
            var request = this._mileageClient.CreateRequest(string.Format("Layout/{0}", layoutName), HttpMethod.Post, layoutData);
            return this._mileageClient.SendRequestAsync(request);
        }

        public Task<HttpResponseMessage> GetLayoutAsync(string layoutName)
        {
            var request = this._mileageClient.CreateRequest(string.Format("Layout/{0}", layoutName), HttpMethod.Get);
            return this._mileageClient.SendRequestAsync(request);
        }
    }
}
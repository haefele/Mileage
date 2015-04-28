using System.Net.Http;
using System.Threading.Tasks;
using DevExpress.Xpo.DB.Helpers;
using LiteGuard;
using Mileage.Client.Contracts.WebServices;

namespace Mileage.Client.Windows.WebServices
{
    public class SearchClient : ISearchClient
    {
        private readonly MileageClient _mileageClient;

        public SearchClient(MileageClient mileageClient)
        {
            Guard.AgainstNullArgument("mileageClient", mileageClient);

            this._mileageClient = mileageClient;
        }

        public Task<HttpResponseMessage> SearchAsync(string searchText, int skip = 0, int take = 50)
        {
            var queryBuilder = new HttpQueryBuilder();
            queryBuilder.AddParameter("searchText", searchText);
            queryBuilder.AddParameter("skip", skip);
            queryBuilder.AddParameter("take", take);

            var request = this._mileageClient.CreateRequest(string.Format("Search{0}", queryBuilder), HttpMethod.Get);
            return this._mileageClient.SendRequestAsync(request);
        }

        public Task<HttpResponseMessage> GetTags(int take = 50)
        {
            var queryBuilder = new HttpQueryBuilder();
            queryBuilder.AddParameter("take", take);

            var request = this._mileageClient.CreateRequest(string.Format("Search/Tags{0}", queryBuilder), HttpMethod.Get);
            return this._mileageClient.SendRequestAsync(request);
        }
    }
}
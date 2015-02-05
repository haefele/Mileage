using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Metrics;
using Metrics.Json;
using Metrics.MetricData;
using Metrics.Utils;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class MetricsController : BaseController
    {
        #region Fields
        private static MetricsDataProvider _dataProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MetricsController"/> class.
        /// </summary>
        static MetricsController()
        {
            Metric.Config.WithConfigExtension((ctx, hs) => _dataProvider = ctx.DataProvider);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="filesSession">The files session.</param>
        public MetricsController(IAsyncDocumentSession documentSession, IAsyncFilesSession filesSession) 
            : base(documentSession, filesSession)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the metrics.
        /// </summary>
        [HttpGet]
        [Route("Metrics")]
        public async Task<HttpResponseMessage> GetMetricsAsync()
        {
            string json = JsonBuilderV2.BuildJson(_dataProvider.CurrentMetricsData);
            var obj = await Task.Run(() => JsonConvert.DeserializeObject(json));

            return this.GetMessageWithObject(HttpStatusCode.Found, obj);
        }
        #endregion
    }
}
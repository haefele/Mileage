using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Metrics;
using Metrics.Json;
using Metrics.MetricData;
using Metrics.Utils;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Extensions;
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
        /// <param name="commandExecutor">The command executor.</param>
        public MetricsController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the metrics.
        /// </summary>
        [HttpGet]
        [Route("Metrics")]
        [IgnoreLicenseValidation]
        [IgnoreVersionValidation]
        public async Task<HttpResponseMessage> GetMetricsAsync()
        {
            string json = JsonBuilderV2.BuildJson(_dataProvider.CurrentMetricsData);
            var obj = await Task.Run(() => JsonConvert.DeserializeObject(json)).WithCurrentCulture();

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, obj);
        }
        #endregion
    }
}
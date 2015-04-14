using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Anotar.NLog;
using LiteGuard;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Contracts.Versioning;
using Mileage.Localization.Client;
using Mileage.Shared.Common;
using Newtonsoft.Json;

namespace Mileage.Client.Windows.WebServices
{
    public class MileageClient
    {
        #region Fields
        private readonly ILocalizationManager _localizationManager;
        private readonly IVersionService _versionService;
        private readonly Session _session;
        private readonly HttpClient _client;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MileageClient"/> class.
        /// </summary>
        /// <param name="localizationManager">The localization manager.</param>
        /// <param name="versionService">The version service.</param>
        /// <param name="session">The current session.</param>
        /// <param name="baseAddress">The base address.</param>
        public MileageClient(ILocalizationManager localizationManager, IVersionService versionService, Session session, string baseAddress)
        {
            Guard.AgainstNullArgument("localizationManager", localizationManager);
            Guard.AgainstNullArgument("versionService", versionService);
            Guard.AgainstNullArgument("session", session);
            Guard.AgainstNullArgument("baseAddress", baseAddress);

            this._localizationManager = localizationManager;
            this._versionService = versionService;
            this._session = session;
            this._client = this.CreateHttpClient(baseAddress);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a request to the specified <paramref name="path"/> with the specified HTTP <paramref name="method"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        public HttpRequestMessage CreateRequest(string path, HttpMethod method, object content = null)
        {
            string contentString = content != null ? JsonConvert.SerializeObject(content) : null;

            var request = new HttpRequestMessage(method, path);

            if (this._session.Token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Mileage", this._session.Token.Token);

            if (string.IsNullOrWhiteSpace(contentString) == false)
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");

            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(this._localizationManager.CurrentLanguage.Name));

            Version currentVersion = this._versionService.GetCurrentVersion();
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(ClientIds.Desktop, currentVersion.ToString())));

            return request;
        }
        /// <summary>
        /// Sends the specified <paramref name="request"/> asynchronously.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                return await this._client.SendAsync(request).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                LogTo.ErrorException("Exception while executing request.", exception);
                return this.CreateNotReachableResponse();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the HTTP client.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        private HttpClient CreateHttpClient(string baseAddress)
        {
            var messageHandler = new HttpClientHandler();
            if (messageHandler.SupportsAutomaticDecompression)
            {
                messageHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return new HttpClient(messageHandler)
            {
                BaseAddress = new Uri(baseAddress)
            };
        }
        /// <summary>
        /// Creates the response when you can't reach the web-service.
        /// </summary>
        private HttpResponseMessage CreateNotReachableResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new ObjectContent(typeof(HttpError),new HttpError(ClientMessages.NotReachable), new JsonMediaTypeFormatter())
            };
        }
        #endregion
    }
}
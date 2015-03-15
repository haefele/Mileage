using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Castle.Core.Logging;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class LoggingMessageHandler : LoggedDelegatingHandler
    {
        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logger = this.GetLogger(request);

            if (logger.IsDebugEnabled)
                logger.DebugFormat("Handling request: {0} | Content: {1}", request, await request.Content.ReadAsStringAsync());

            HttpResponseMessage result = await base.SendAsync(request, cancellationToken);

            if (logger.IsDebugEnabled)
                logger.DebugFormat("Sending response: {0} | Content: {1}", result, await result.Content.ReadAsStringAsync());

            return result;
        }
        #endregion
    }
}
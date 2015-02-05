using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Castle.Core.Logging;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        #region Fields
        private ILogger _logger;
        #endregion

        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logger = this.GetLogger(request.GetDependencyScope());

            if (logger.IsDebugEnabled)
                logger.DebugFormat("Handling request: {0}{1}Content:{1}{2}", request, Environment.NewLine, await request.Content.ReadAsStringAsync());

            HttpResponseMessage result = await base.SendAsync(request, cancellationToken);

            if (logger.IsDebugEnabled)
                logger.DebugFormat("Sending response: {0}{1}Content:{1}{2}", result, await result.Content.ReadAsStringAsync());

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="dependencyScope">The dependency scope.</param>
        private ILogger GetLogger(IDependencyScope dependencyScope)
        {
            if (this._logger != null)
                return this._logger;

            //This is not thread-safe
            //But at this point, it doesn't really matter
            var loggerFactory = dependencyScope.GetService<ILoggerFactory>();
            this._logger = loggerFactory.Create(this.GetType());

            return this._logger;
        }
        #endregion
    }
}
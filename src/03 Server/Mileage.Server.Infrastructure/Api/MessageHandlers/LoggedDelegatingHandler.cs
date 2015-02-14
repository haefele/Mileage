using System.Net.Http;
using System.Web.Http.Dependencies;
using Castle.Core.Logging;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public abstract class LoggedDelegatingHandler : DelegatingHandler
    {
        #region Fields
        private ILogger _logger;
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="request">The request.</param>
        protected ILogger GetLogger(HttpRequestMessage request)
        {
            if (this._logger != null)
                return this._logger;

            //This is not thread-safe
            //But at this point, it doesn't really matter
            var loggerFactory = request.GetDependencyScope().GetService<ILoggerFactory>();
            this._logger = loggerFactory.Create(this.GetType());

            return this._logger;
        }
        #endregion
    }
}
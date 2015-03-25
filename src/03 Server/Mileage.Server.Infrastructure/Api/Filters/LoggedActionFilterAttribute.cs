using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Castle.Core.Logging;
using JetBrains.Annotations;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.Filters
{
    public abstract class LoggedActionFilterAttribute : ActionFilterAttribute
    {
        #region Fields
        private ILogger _logger;
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="context">The context.</param>
        [NotNull]
        protected ILogger GetLogger([NotNull]HttpActionContext context)
        {
            if (this._logger != null)
                return this._logger;

            //This is not thread-safe
            //But at this point, it doesn't really matter
            var loggerFactory = context.Request.GetDependencyScope().GetService<ILoggerFactory>();
            this._logger = loggerFactory.Create(this.GetType());

            return this._logger;
        }
        #endregion
    }
}
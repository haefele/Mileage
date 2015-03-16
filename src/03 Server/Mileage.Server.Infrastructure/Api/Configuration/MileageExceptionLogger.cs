using System.Web.Http.ExceptionHandling;
using Castle.Core.Logging;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.Configuration
{
    public class MileageExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// When overridden in a derived class, logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            var loggerFactory = context.RequestContext.Configuration.DependencyResolver.GetService<ILoggerFactory>();
            var logger = loggerFactory.Create("Mileage.GlobalExceptionHandler");

            logger.Error(string.Format("Unhandled exception. Returning 501 Internal Server Error. Catch block: {0}", context.CatchBlock), context.Exception);
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Anotar.NLog;
using Mileage.Localization.Server;
using Mileage.Server.Contracts.Versioning;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.Filters
{
    public class VersionValidationFilter : IActionFilter
    {
        /// <summary>
        /// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        public bool AllowMultiple
        {
            get { return false; }
        }
        /// <summary>
        /// Executes the filter action asynchronously.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="cancellationToken">The cancellation token assigned for this task.</param>
        /// <param name="continuation">The delegate function to continue after the action method is invoked.</param>
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreVersionValidationAttribute>().Any())
                return await continuation();

            string clientVersionString = actionContext.Request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Version).FirstOrDefault();

            Version clientVersion;
            if (Version.TryParse(clientVersionString, out clientVersion) == false)
            {
                LogTo.Warn("Blocked incoming request with an invalid UserAgent client version. Client IP: {0}", actionContext.Request.GetOwinContext().Request.RemoteIpAddress);
                return actionContext.Request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.UnknownClient);
            }

            Version serverVersion = actionContext.Request.GetDependencyScope().GetService<IVersionService>().GetCurrentVersion();
            if (clientVersion < serverVersion)
            {
                LogTo.Warn("Blocked incoming request. Client has outdated version '{0}'. Client IP: {1}", clientVersionString, actionContext.Request.GetOwinContext().Request.RemoteIpAddress);
                return actionContext.Request.GetMessageWithError(HttpStatusCode.UpgradeRequired, ServerMessages.OutdatedVersion);
            }

            return await continuation();
        }
    }
}
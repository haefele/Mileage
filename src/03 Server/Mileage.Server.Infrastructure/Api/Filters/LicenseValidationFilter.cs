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
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Filters
{
    public class LicenseValidationFilter : IActionFilter
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
            if (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreLicenseValidationAttribute>().Any())
                return await continuation();

            string clientId = actionContext.Request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Name).FirstOrDefault();
            if (clientId == null)
            {
                LogTo.Warn("Blocked incoming request without an UserAgent header. Client IP: {0}", actionContext.Request.GetOwinContext().Request.RemoteIpAddress);
                return actionContext.Request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.UnknownClient);
            }

            Result licenseResult = actionContext.Request.GetDependencyScope().GetService<ILicensingService>().AssertValidLicense(clientId);
            if (licenseResult.IsError)
            {
                LogTo.Warn("Blocked incoming request. No license for the client '{0}'. Client IP: {1}", clientId, actionContext.Request.GetOwinContext().Request.RemoteIpAddress);
                return actionContext.Request.GetMessageWithError(HttpStatusCode.Forbidden, licenseResult.Message);
            }

            return await continuation();
        }
    }
}
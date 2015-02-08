using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core.Logging;
using Mileage.Localization.Server;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class LicenseValidationMessageHandler : LoggedDelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ILogger logger = this.GetLogger(request);

            string clientId = request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Name).FirstOrDefault();
            if (clientId == null)
            { 
                logger.WarnFormat("Blocked incoming request without an UserAgent header. Client IP: {0}", request.GetOwinContext().Request.RemoteIpAddress);
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ServerMessages.UnknownClient));
            }

            Result licenseResult = request.GetDependencyScope().GetService<ILicensingService>().AssertValidLicense(clientId);
            if (licenseResult.IsError)
            { 
                logger.WarnFormat("Blocked incoming request. No license for the client '{0}'. Client IP: {1}", clientId, request.GetOwinContext().Request.RemoteIpAddress);
                return request.CreateErrorResponse(HttpStatusCode.Forbidden, new HttpError(licenseResult.Message));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
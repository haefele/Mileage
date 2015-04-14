using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Anotar.NLog;
using Mileage.Localization.Server;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class LicenseValidationMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string clientId = request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Name).FirstOrDefault();
            if (clientId == null)
            {
                LogTo.Warn("Blocked incoming request without an UserAgent header. Client IP: {0}", request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.UnknownClient);
            }

            Result licenseResult = request.GetDependencyScope().GetService<ILicensingService>().AssertValidLicense(clientId);
            if (licenseResult.IsError)
            {
                LogTo.Warn("Blocked incoming request. No license for the client '{0}'. Client IP: {1}", clientId, request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.Forbidden, licenseResult.Message);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Mileage.Localization.Server;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Contracts.Versioning;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class VersionValidationMessageHandler : LoggedDelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ILogger logger = this.GetLogger(request);

            Version clientVersion;
            string clientVersionString = request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Version).FirstOrDefault();
            if (Version.TryParse(clientVersionString, out clientVersion) == false)
            {
                logger.WarnFormat("Blocked incoming request with an invalid UserAgent client version. Client IP: {0}", request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.UnknownClient);
            }

            Version serverVersion = request.GetDependencyScope().GetService<IVersionService>().GetCurrentVersion();
            if (clientVersion < serverVersion)
            {
                logger.WarnFormat("Blocked incoming request. Client has outdated version '{0}'. Client IP: {1}", clientVersionString, request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.Forbidden, ServerMessages.OutdatedVersion);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
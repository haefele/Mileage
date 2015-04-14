using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Anotar.NLog;
using Mileage.Localization.Server;
using Mileage.Server.Contracts.Versioning;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.MessageHandlers
{
    public class VersionValidationMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string clientVersionString = request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Version).FirstOrDefault();

            Version clientVersion;
            if (Version.TryParse(clientVersionString, out clientVersion) == false)
            {
                LogTo.Warn("Blocked incoming request with an invalid UserAgent client version. Client IP: {0}", request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.UnknownClient);
            }

            Version serverVersion = request.GetDependencyScope().GetService<IVersionService>().GetCurrentVersion();
            if (clientVersion < serverVersion)
            {
                LogTo.Warn("Blocked incoming request. Client has outdated version '{0}'. Client IP: {1}", clientVersionString, request.GetOwinContext().Request.RemoteIpAddress);
                return request.GetMessageWithError(HttpStatusCode.UpgradeRequired, ServerMessages.OutdatedVersion);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
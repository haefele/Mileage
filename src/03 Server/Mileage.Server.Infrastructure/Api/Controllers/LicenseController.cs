using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Licensing;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class LicenseController : BaseController
    {
        #region Fields
        private readonly ILicensingService _licensingService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        /// <param name="licensingService">The licensing service.</param>
        public LicenseController(ICommandExecutor commandExecutor, ILicensingService licensingService)
            : base(commandExecutor)
        {
            Guard.AgainstNullArgument("licensingService", licensingService);

            this._licensingService = licensingService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the license.
        /// </summary>
        [HttpGet]
        [Route("License")]
        [IgnoreVersionValidation]
        [IgnoreLicenseValidation]
        public HttpResponseMessage GetLicense()
        {
            Result<LicenseInfo> licenseInfo = this._licensingService.GetLicenseInfo();
            return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.InternalServerError, licenseInfo);
        }
        #endregion
    }
}
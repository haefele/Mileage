using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Localization.Server.Controllers;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands.Authentication;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Models;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Authentication")]
    public class AuthenticationController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public AuthenticationController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="loginDataData">The login data.</param>
        /// <returns>
        /// 200 - OK: Login is successfull.
        /// 400 - BadRequest: Required data are missing.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginAsync(LoginData loginDataData)
        {
            if (loginDataData == null || loginDataData.EmailAddress == null || loginDataData.PasswordMD5Hash == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ControllerMessages.RequiredDataAreMissing);

            var userAgent = this.Request.Headers.UserAgent.Select(f => f.Product).First();
            
            Result<AuthenticationToken> result = await this.CommandExecutor.Execute(new ValidateLoginAndCreateTokenCommand(
                loginDataData.EmailAddress,
                loginDataData.PasswordMD5Hash,
                userAgent.Name,
                userAgent.Version,
                this.OwinContext.Request.RemoteIpAddress));

            return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result);
        }
        #endregion
    }
}
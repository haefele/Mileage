using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Metrics.Core;
using Mileage.Localization.Server.Login;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities;
using Mileage.Shared.Extensions;
using Mileage.Shared.Models;
using Mileage.Shared.Results;
using NLog.Targets.Wrappers;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class LoginController : BaseController
    {
        #region Fields
        private readonly ILicensingService _licensingService;
        private readonly ISaltCombiner _saltCombiner;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="filesSession">The files session.</param>
        /// <param name="licensingService">The licensing service.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        public LoginController(IAsyncDocumentSession documentSession, IAsyncFilesSession filesSession, ILicensingService licensingService, ISaltCombiner saltCombiner)
            : base(documentSession, filesSession)
        {
            this._licensingService = licensingService;
            this._saltCombiner = saltCombiner;
        }
        #endregion

        /// <summary>
        /// Logins the specified login data.
        /// </summary>
        /// <param name="loginData">The login data.</param>
        /// <returns>
        /// 200 - OK: Login is successfull.
        /// 400 - BadRequest: Required data are missing.
        /// 403 - Forbidden: No license for the application.
        /// 404 - NotFound: Username and password are not correct.
        /// </returns>
        public async Task<HttpResponseMessage> Login(Login loginData)
        {
            if (loginData == null || loginData.Username == null || loginData.PasswordMD5Hash == null)
                return this.GetMessageWithError(HttpStatusCode.BadRequest, LoginMessages.LoginDataMissing);

            string clientId = this.Request.Headers.UserAgent.Select(f => f.Product).Select(f => f.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(clientId))
                return this.GetMessageWithError(HttpStatusCode.BadRequest, LoginMessages.UnknownClient);

            Result licenseResult = this._licensingService.AssertValidLicense(clientId);

            if (licenseResult.IsError)
                return this.GetMessageWithResult(HttpStatusCode.Forbidden, licenseResult);

            User user = await this.DocumentSession.Query<User, UsersByUsername>()
                .Where(f => f.Username == loginData.Username)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return this.GetMessageWithError(HttpStatusCode.NotFound, LoginMessages.UserNotFound);

            AuthenticationData authenticationData = await this.DocumentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            byte[] passedHash = this._saltCombiner.Combine(authenticationData.Salt, loginData.PasswordMD5Hash);

            if (authenticationData.Hash.SequenceEqual(passedHash) == false)
                return this.GetMessageWithError(HttpStatusCode.NotFound, LoginMessages.PasswordIncorrect);

            return this.GetMessageWithObject(HttpStatusCode.OK, new {Token = "asdf"});
        }
    }
}
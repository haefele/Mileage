using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using LiteGuard;
using Metrics.Core;
using Mileage.Localization.Server.Login;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Common;
using Mileage.Shared.Entities;
using Mileage.Shared.Extensions;
using Mileage.Shared.Models;
using Mileage.Shared.Results;
using NLog.Targets.Wrappers;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        #region Constants
        /// <summary>
        /// The duration a token is valid.
        /// </summary>
        public const int ValidTokenDurationInHours = 12;
        #endregion

        #region Fields
        private readonly ILicensingService _licensingService;
        private readonly ISaltCombiner _saltCombiner;
        private readonly ISecretGenerator _secretGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="filesSession">The files session.</param>
        /// <param name="licensingService">The licensing service.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        public AuthenticationController(IAsyncDocumentSession documentSession, IAsyncFilesSession filesSession, ILicensingService licensingService, ISaltCombiner saltCombiner, ISecretGenerator secretGenerator)
            : base(documentSession, filesSession)
        {
            Guard.AgainstNullArgument("licensingService", licensingService);
            Guard.AgainstNullArgument("saltCombiner", saltCombiner);
            Guard.AgainstNullArgument("secretGenerator", secretGenerator);

            this._licensingService = licensingService;
            this._saltCombiner = saltCombiner;
            this._secretGenerator = secretGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="loginData">The login data.</param>
        /// <returns>
        /// 200 - OK: Login is successfull.
        /// 400 - BadRequest: Required data are missing.
        /// 404 - NotFound: Username and password are not correct.
        /// 406 - NotAcceptable: User is deactivated.
        /// </returns>
        [HttpPost]
        [Route("Authentication/Login")]
        public async Task<HttpResponseMessage> Login(Login loginData)
        {
            if (loginData == null || loginData.Username == null || loginData.PasswordMD5Hash == null)
                return this.GetMessageWithError(HttpStatusCode.BadRequest, LoginMessages.LoginDataMissing);
            
            User user = await this.GetUserWithUsername(loginData.Username).WithCurrentCulture();

            if (user == null)
                return this.GetMessageWithError(HttpStatusCode.NotFound, LoginMessages.UserNotFound);

            if (user.IsDeactivated)
                return this.GetMessageWithError(HttpStatusCode.NotAcceptable, LoginMessages.UserIsDeactivated);

            AuthenticationData authenticationData = await this.DocumentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            byte[] passedHash = this._saltCombiner.Combine(authenticationData.Salt, loginData.PasswordMD5Hash);
            
            if (authenticationData.Hash.SequenceEqual(passedHash) == false)
                return this.GetMessageWithError(HttpStatusCode.NotFound, LoginMessages.PasswordIncorrect);

            var client = this.Request.Headers.UserAgent.Select(f => f.Product).First();

            var token = new AuthenticationToken
            {
                Token = this._secretGenerator.GenerateString(),
                CreatedDate = DateTimeOffset.Now,
                UserId = user.Id,
                ValidUntil = DateTimeOffset.Now.AddHours(ValidTokenDurationInHours),
                Client = new Client
                {
                    ClientId = client.Name,
                    Version = client.Version,
                    IP = this.OwinContext.Request.RemoteIpAddress
                }
            };

            await this.DocumentSession.StoreAsync(token).WithCurrentCulture();

            return this.GetMessageWithObject(HttpStatusCode.OK, token);
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerData">The register data.</param>
        /// <returns>
        /// 201 - Created: The user was created.
        /// 400 - BadRequest: Required data are missing.
        /// 403 - Forbidden: Email address is in use.
        /// </returns>
        [HttpPost]
        [Route("Authentication/Register")]
        public async Task<HttpResponseMessage> Register(Register registerData)
        {
            if (registerData == null || registerData.EmailAddress == null || registerData.Username == null || registerData.PasswordMD5Hash == null || registerData.Language == null)
                return this.GetMessageWithError(HttpStatusCode.BadRequest, LoginMessages.RegisterDataMissing);

            if (await this.IsEmailAddressInUse(registerData.EmailAddress).WithCurrentCulture())
                return this.GetMessageWithError(HttpStatusCode.Forbidden, LoginMessages.EmailIsNotAvailable);

            var user = new User
            {
                IsDeactivated = true,
                NotificationEmailAddress = registerData.EmailAddress,
                PreferredLanguage = registerData.Language,
                Username = registerData.Username
            };

            await this.DocumentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                UserId = user.Id,
                Salt = this._secretGenerator.Generate()
            };
            authenticationData.Hash = this._saltCombiner.Combine(authenticationData.Salt, registerData.PasswordMD5Hash);

            await this.DocumentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return this.GetMessageWithObject(HttpStatusCode.Created, user);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the user with the specified <paramref name="username"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        private Task<User> GetUserWithUsername(string username)
        {
            return this.DocumentSession.Query<User, UsersForQuery>()
                .Where(f => f.Username == username)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Returns whether the specified <paramref name="emailAddress"/> is already in use.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private Task<bool> IsEmailAddressInUse(string emailAddress)
        {
            return this.DocumentSession.Query<User, UsersForQuery>()
                .Where(f => f.NotificationEmailAddress == emailAddress)
                .AnyAsync();
        }
        #endregion
    }
}
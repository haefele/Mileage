using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteGuard;
using Mileage.Localization.Server.Authentication;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Infrastructure.Commands.Mileage;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Authentication
{
    public class ValidateLoginAndCreateTokenCommand : ICommand<AuthenticationToken>
    {
        public ValidateLoginAndCreateTokenCommand(string emailAddress, byte[] passwordMD5Hash, string clientId, string clientVersion, string clientIP)
        {
            Guard.AgainstNullArgument("EmailAddress", emailAddress);
            Guard.AgainstNullArgument("passwordMD5Hash", passwordMD5Hash);
            Guard.AgainstNullArgument("clientId", clientId);
            Guard.AgainstNullArgument("clientVersion", clientVersion);
            Guard.AgainstNullArgument("clientIP", clientId);

            this.EmailAddress = emailAddress;
            this.PasswordMD5Hash = passwordMD5Hash;
            this.ClientId = clientId;
            this.ClientVersion = clientVersion;
            this.ClientIP = clientIP;
        }

        public string EmailAddress { get; private set; }
        public byte[] PasswordMD5Hash { get; private set; }
        public string ClientId { get; private set; }
        public string ClientVersion { get; private set; }
        public string ClientIP { get; private set; }
    }

    public class ValidateLoginAndCreateTokenCommandHandler : CommandHandler<ValidateLoginAndCreateTokenCommand, AuthenticationToken>
    {
        #region Constants
        /// <summary>
        /// The duration a token is valid.
        /// </summary>
        public const int ValidTokenDurationInHours = 12;
        #endregion

        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISaltCombiner _saltCombiner;
        private readonly ISecretGenerator _secretGenerator;

        public ValidateLoginAndCreateTokenCommandHandler(IAsyncDocumentSession documentSession, ISaltCombiner saltCombiner, ISecretGenerator secretGenerator)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);
            Guard.AgainstNullArgument("saltCombiner", saltCombiner);
            Guard.AgainstNullArgument("secretGenerator", secretGenerator);

            this._documentSession = documentSession;
            this._saltCombiner = saltCombiner;
            this._secretGenerator = secretGenerator;
        }

        public override async Task<Result<AuthenticationToken>> Execute(ValidateLoginAndCreateTokenCommand command, ICommandScope scope)
        {
            Result<User> userResult = await this.GetUserWithEmailAddress(command.EmailAddress, scope).WithCurrentCulture();

            if (userResult.IsError)
                return Result.AsError(userResult.Message);

            if (userResult.Data.IsDeactivated)
                return Result.AsError(AuthenticationMessages.UserIsDeactivated);
            
            AuthenticationData authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(userResult.Data.Id))
                .WithCurrentCulture();

            byte[] passedHash = this._saltCombiner.Combine(authenticationData.Salt, command.PasswordMD5Hash);

            if (authenticationData.Hash.SequenceEqual(passedHash) == false)
                return Result.AsError(AuthenticationMessages.PasswordIncorrect);
            
            var token = new AuthenticationToken
            {
                Token = this._secretGenerator.GenerateString(),
                CreatedDate = DateTimeOffset.Now,
                UserId = userResult.Data.Id,
                ValidUntil = DateTimeOffset.Now.AddHours(ValidTokenDurationInHours),
                Client = new Client
                {
                    ClientId = command.ClientId,
                    Version = command.ClientVersion,
                    IP = command.ClientIP
                }
            };

            await this._documentSession.StoreAsync(token).WithCurrentCulture();

            return Result.AsSuccess(token);
        }

        /// <summary>
        /// Returns the user with the specified <paramref name="emailAddress"/>.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="scope">The command scope.</param>
        private async Task<Result<User>> GetUserWithEmailAddress(string emailAddress, ICommandScope scope)
        {
            User userByFullEmailAddress = await this._documentSession.Query<User, UsersForQuery>()
                .Where(f => f.EmailAddress == emailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (userByFullEmailAddress != null)
                return Result.AsSuccess(userByFullEmailAddress);

            Result<MileageSettings> mileageSettingsCommand = await scope.Execute(new GetMileageSettingsCommand());

            if (mileageSettingsCommand.IsError)
                return Result.AsError(mileageSettingsCommand.Message);

            string fullEmailAddress = string.Format("{0}@{1}", emailAddress, mileageSettingsCommand.Data.DefaultEmailSuffix);

            User userByConstructedEmailAddress = await this._documentSession.Query<User, UsersForQuery>()
                .Where(f => f.EmailAddress == fullEmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (userByConstructedEmailAddress != null)
                return Result.AsSuccess(userByConstructedEmailAddress);

            return Result.AsError(AuthenticationMessages.UserNotFound);
        }
    }
}
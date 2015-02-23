using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteGuard;
using Mileage.Localization.Server.Authentication;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Authentication
{
    public class ValidateLoginAndCreateTokenCommand : ICommand<AuthenticationToken>
    {
        public ValidateLoginAndCreateTokenCommand(string username, byte[] passwordMD5Hash, string clientId, string clientVersion, string clientIP)
        {
            Guard.AgainstNullArgument("username", username);
            Guard.AgainstNullArgument("passwordMD5Hash", passwordMD5Hash);
            Guard.AgainstNullArgument("clientId", clientId);
            Guard.AgainstNullArgument("clientVersion", clientVersion);
            Guard.AgainstNullArgument("clientIP", clientId);

            this.Username = username;
            this.PasswordMD5Hash = passwordMD5Hash;
            this.ClientId = clientId;
            this.ClientVersion = clientVersion;
            this.ClientIP = clientIP;
        }

        public string Username { get; private set; }
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
            User user = await this.GetUserWithUsername(command.Username).WithCurrentCulture();

            if (user == null)
                return Result.AsError(AuthenticationMessages.UserNotFound);

            if (user.IsDeactivated)
                return Result.AsError(AuthenticationMessages.UserIsDeactivated);
            
            AuthenticationData authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            byte[] passedHash = this._saltCombiner.Combine(authenticationData.Salt, command.PasswordMD5Hash);

            if (authenticationData.Hash.SequenceEqual(passedHash) == false)
                return Result.AsError(AuthenticationMessages.PasswordIncorrect);
            
            var token = new AuthenticationToken
            {
                Token = this._secretGenerator.GenerateString(),
                CreatedDate = DateTimeOffset.Now,
                UserId = user.Id,
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
        /// Returns the user with the specified <paramref name="username"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        private Task<User> GetUserWithUsername(string username)
        {
            return this._documentSession.Query<User, UsersForQuery>()
                .Where(f => f.Username == username)
                .FirstOrDefaultAsync();
        }
    }
}
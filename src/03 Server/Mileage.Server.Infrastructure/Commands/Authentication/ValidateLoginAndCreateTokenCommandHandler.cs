using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Authentication;
using Mileage.Server.Contracts.Commands.Mileage;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Infrastructure.Commands.Mileage;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Authentication
{
    public class ValidateLoginAndCreateTokenCommandHandler : CommandHandler<ValidateLoginAndCreateTokenCommand, AuthenticationToken>
    {
        #region Constants
        /// <summary>
        /// The duration a token is valid.
        /// </summary>
        public const int ValidTokenDurationInHours = 12;
        #endregion

        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISaltCombiner _saltCombiner;
        private readonly ISecretGenerator _secretGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateLoginAndCreateTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        public ValidateLoginAndCreateTokenCommandHandler(IAsyncDocumentSession documentSession, ISaltCombiner saltCombiner, ISecretGenerator secretGenerator)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);
            Guard.AgainstNullArgument("saltCombiner", saltCombiner);
            Guard.AgainstNullArgument("secretGenerator", secretGenerator);

            this._documentSession = documentSession;
            this._saltCombiner = saltCombiner;
            this._secretGenerator = secretGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public override async Task<Result<AuthenticationToken>> Execute(ValidateLoginAndCreateTokenCommand command, ICommandScope scope)
        {
            Result<User> userResult = await this.GetUserWithEmailAddress(command.EmailAddress, scope).WithCurrentCulture();

            if (userResult.IsError)
                return Result.AsError(userResult.Message);

            if (userResult.Data.IsDeactivated)
                return Result.AsError(CommandMessages.UserIsDeactivated);
            
            AuthenticationData authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(userResult.Data.Id))
                .WithCurrentCulture();

            byte[] passedHash = this._saltCombiner.Combine(authenticationData.Salt, command.PasswordMD5Hash);

            if (authenticationData.Hash.SequenceEqual(passedHash) == false)
                return Result.AsError(CommandMessages.PasswordIncorrect);
            
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the user with the specified <paramref name="emailAddress"/>.
        /// This works in a three step process:
        /// 1. Exact given <paramref name="emailAddress"/>.
        /// 2. Append the <see cref="MileageSettings.DefaultEmailSuffix"/> to the given <paramref name="emailAddress"/>.
        /// 3. Replace everything after the <c>@</c> in the <paramref name="emailAddress"/> with the <see cref="MileageSettings.DefaultEmailSuffix"/>.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="scope">The command scope.</param>
        private async Task<Result<User>> GetUserWithEmailAddress(string emailAddress, ICommandScope scope)
        {
            User userByGivenEmailAddress = await this._documentSession.Query<User, UsersForSearch>()
                .Where(f => f.EmailAddress == emailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (userByGivenEmailAddress != null)
                return Result.AsSuccess(userByGivenEmailAddress);

            Result<MileageSettings> mileageSettingsCommand = await scope.Execute(new GetMileageSettingsCommand());

            if (mileageSettingsCommand.IsError)
                return Result.AsError(mileageSettingsCommand.Message);

            string appendedEmailAddress = emailAddress.AppendEmailSuffix(mileageSettingsCommand.Data.DefaultEmailSuffix);

            User userByAppendedEmailAddress = await this._documentSession.Query<User, UsersForSearch>()
                .Where(f => f.EmailAddress == appendedEmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (userByAppendedEmailAddress != null)
                return Result.AsSuccess(userByAppendedEmailAddress);

            string replacedEmailAddress = emailAddress.UpdateEmailSuffix(mileageSettingsCommand.Data.DefaultEmailSuffix);

            User userByReplacedEmailAddress = await this._documentSession.Query<User, UsersForSearch>()
                .Where(f => f.EmailAddress == replacedEmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (userByReplacedEmailAddress != null)
                return Result.AsSuccess(userByReplacedEmailAddress);

            return Result.AsError(CommandMessages.UserNotFound);
        }
        #endregion
    }
}
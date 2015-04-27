using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Users;
using Mileage.Server.Contracts.Encryption;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.Linq;

namespace Mileage.Server.Infrastructure.Commands.Users
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        public CreateUserCommandHandler(IAsyncDocumentSession documentSession, ISecretGenerator secretGenerator, ISaltCombiner saltCombiner)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);
            Guard.AgainstNullArgument("secretGenerator", secretGenerator);
            Guard.AgainstNullArgument("saltCombiner", saltCombiner);

            this._documentSession = documentSession;
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<Result<User>> Execute(CreateUserCommand command, ICommandScope scope)
        {
            if (await this.IsEmailAddressInUse(command.EmailAddress).WithCurrentCulture())
                return Result.AsError(CommandMessages.EmailIsNotAvailable);

            var user = new User
            {
                EmailAddress = command.EmailAddress,
                IsDeactivated = true,
                PreferredLanguage = command.Language,
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                Id = AuthenticationData.CreateId(user.Id),
                UserId = user.Id,
                Salt = this._secretGenerator.Generate()
            };
            authenticationData.Hash = this._saltCombiner.Combine(authenticationData.Salt, command.PasswordMD5Hash);

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return Result.AsSuccess(user);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns whether the specified <paramref name="emailAddress"/> is already in use.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private Task<bool> IsEmailAddressInUse(string emailAddress)
        {
            return this._documentSession.Query<User, UsersByEmailAddress>()
                .Where(f => f.EmailAddress == emailAddress)
                .AnyAsync();
        }
        #endregion
    }
}
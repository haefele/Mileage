﻿using System.Threading.Tasks;
using LiteGuard;
using Metrics.Core;
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
using Raven.Client.Linq;

namespace Mileage.Server.Infrastructure.Commands.Users
{
    public class CreateUserCommand : ICommand<User>
    {
        public CreateUserCommand(string username, byte[] passwordMD5Hash, string emailAddress, string language)
        {
            Guard.AgainstNullArgument("username", username);
            Guard.AgainstNullArgument("passwordMD5Hash", passwordMD5Hash);
            Guard.AgainstNullArgument("emailAddress", emailAddress);
            Guard.AgainstNullArgument("language", language);

            this.Username = username;
            this.PasswordMD5Hash = passwordMD5Hash;
            this.EmailAddress = emailAddress;
            this.Language = language;
        }

        public string Username { get; private set; }
        public byte[] PasswordMD5Hash { get; private set; }
        public string EmailAddress { get; private set; }
        public string Language { get; private set; }
    }

    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, User>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;

        public CreateUserCommandHandler(IAsyncDocumentSession documentSession, ISecretGenerator secretGenerator, ISaltCombiner saltCombiner)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);
            Guard.AgainstNullArgument("secretGenerator", secretGenerator);
            Guard.AgainstNullArgument("saltCombiner", saltCombiner);

            this._documentSession = documentSession;
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
        }

        public override async Task<Result<User>> Execute(CreateUserCommand command, ICommandScope scope)
        {
            if (await this.IsEmailAddressInUse(command.EmailAddress).WithCurrentCulture())
                return Result.AsError(AuthenticationMessages.EmailIsNotAvailable);

            var user = new User
            {
                IsDeactivated = true,
                NotificationEmailAddress = command.EmailAddress,
                PreferredLanguage = command.Language,
                Username = command.Username
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                UserId = user.Id,
                Salt = this._secretGenerator.Generate()
            };
            authenticationData.Hash = this._saltCombiner.Combine(authenticationData.Salt, command.PasswordMD5Hash);

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return Result.AsSuccess(user);
        }

        /// <summary>
        /// Returns whether the specified <paramref name="emailAddress"/> is already in use.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private Task<bool> IsEmailAddressInUse(string emailAddress)
        {
            return this._documentSession.Query<User, UsersForQuery>()
                .Where(f => f.NotificationEmailAddress == emailAddress)
                .AnyAsync();
        }
    }
}
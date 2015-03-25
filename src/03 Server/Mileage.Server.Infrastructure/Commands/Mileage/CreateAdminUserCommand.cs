using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands.Users;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class CreateAdminUserCommand : ICommand<User>
    {
        public CreateAdminUserCommand([NotNull]string emailAddress, [NotNull]byte[] passwordMD5Hash, [NotNull]string language)
        {
            Guard.AgainstNullArgument("emailAddress", emailAddress);
            Guard.AgainstNullArgument("passwordMD5Hash", passwordMD5Hash);
            Guard.AgainstNullArgument("language", language);

            this.EmailAddress = emailAddress;
            this.PasswordMD5Hash = passwordMD5Hash;
            this.Language = language;
        }

        public string EmailAddress { get; private set; }
        public byte[] PasswordMD5Hash { get; private set; }
        public string Language { get; private set; }
    }

    public class CreateAdminUserCommandHandler : CommandHandler<CreateAdminUserCommand, User>
    {
        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public override async Task<Result<User>> Execute(CreateAdminUserCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument("command", command);
            Guard.AgainstNullArgument("scope", scope);

            Result<MileageInternalSettings> mileageInternalSettingsResult = await scope.Execute(new GetMileageInternalSettingsCommand());

            if (mileageInternalSettingsResult.IsError)
                return Result.AsError(mileageInternalSettingsResult.Message);

            if (mileageInternalSettingsResult.Data.IsAdminUserCreated)
                return Result.AsError(CommandMessages.AdminUserAlreadyCreated);

            Result<User> createUserResult = await scope.Execute(new CreateUserCommand(command.EmailAddress, command.PasswordMD5Hash, command.Language));

            if (createUserResult.IsError)
                return createUserResult;

            //The admin user is not deactivated
            //This is the first user we create, so someone needs to be able to login
            createUserResult.Data.IsDeactivated = false;

            mileageInternalSettingsResult.Data.IsAdminUserCreated = true;

            return createUserResult;
        }
        #endregion
    }
}
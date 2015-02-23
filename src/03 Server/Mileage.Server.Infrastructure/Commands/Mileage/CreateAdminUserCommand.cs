using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands.Users;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class CreateAdminUserCommand : ICommand<User>
    {
        public CreateAdminUserCommand(string username, byte[] passwordMD5Hash, string emailAddress, string language)
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

    public class CreateAdminUserCommandHandler : CommandHandler<CreateAdminUserCommand, User>
    {
        public override async Task<Result<User>> Execute(CreateAdminUserCommand command, ICommandScope scope)
        {
            Result<MileageSettings> mileageSettingsResult = await scope.Execute(new GetMileageSettingsCommand());

            if (mileageSettingsResult.IsError)
                return Result.AsError(mileageSettingsResult.Message);

            if (mileageSettingsResult.Data.IsAdminUserCreated)
                return Result.AsError("Admin user already created.");

            Result<User> createUserResult = await scope.Execute(new CreateUserCommand(command.Username, command.PasswordMD5Hash, command.EmailAddress, command.Language));

            if (createUserResult.IsError)
                return createUserResult;

            //The admin user is not deactivated
            //This is the first user we create, so someone needs to be able to login
            createUserResult.Data.IsDeactivated = false;

            mileageSettingsResult.Data.IsAdminUserCreated = true;

            return createUserResult;
        }
    }
}
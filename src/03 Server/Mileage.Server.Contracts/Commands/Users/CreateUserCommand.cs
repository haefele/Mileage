using JetBrains.Annotations;
using LiteGuard;
using Mileage.Shared.Entities.Users;

namespace Mileage.Server.Contracts.Commands.Users
{
    public class CreateUserCommand : ICommand<User>
    {
        public CreateUserCommand([NotNull]string emailAddress, [NotNull]byte[] passwordMD5Hash, [NotNull]string language)
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
}
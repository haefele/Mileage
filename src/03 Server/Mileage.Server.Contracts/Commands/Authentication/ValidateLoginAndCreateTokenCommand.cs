using JetBrains.Annotations;
using LiteGuard;
using Mileage.Shared.Entities.Authentication;

namespace Mileage.Server.Contracts.Commands.Authentication
{
    public class ValidateLoginAndCreateTokenCommand : ICommand<AuthenticationToken>
    {
        public ValidateLoginAndCreateTokenCommand([NotNull]string emailAddress, [NotNull]byte[] passwordMD5Hash, [NotNull]string clientId, [NotNull]string clientVersion, [NotNull]string clientIP)
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
}
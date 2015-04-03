using JetBrains.Annotations;
using LiteGuard;
using Microsoft.Owin;

namespace Mileage.Server.Contracts.Commands.Authentication
{
    public class AuthenticateCommand : ICommand<string>
    {
        public IOwinContext OwinContext { get; private set; }

        public AuthenticateCommand([NotNull]IOwinContext owinContext)
        {
            Guard.AgainstNullArgument("owinContext", owinContext);

            this.OwinContext = owinContext;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Users
{
    public class GetCurrentUserCommand : ICommand<User>
    {
         
    }

    public class GetCurrentUserCommandHandler : CommandHandler<GetCurrentUserCommand, User>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetCurrentUserCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }

        public override async Task<Result<User>> Execute(GetCurrentUserCommand command, ICommandScope scope)
        {
            string currentUserId = Thread.CurrentPrincipal.Identity.Name;
            var currentUser = await this._documentSession.LoadAsync<User>(currentUserId).WithCurrentCulture();

            return Result.AsSuccess(currentUser);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Layout;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Layout
{
    public class SaveLayoutCommandHandler : CommandHandler<SaveLayoutCommand, object>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public SaveLayoutCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }

        public override async Task<Result<object>> Execute(SaveLayoutCommand command, ICommandScope scope)
        {
            var storedLayout = new StoredLayout
            {
                Id = StoredLayout.CreateId(command.UserId, command.LayoutName),
                LayoutName = command.LayoutName,
                LayoutData = command.LayoutData,
                UserId = command.UserId
            };

            await this._documentSession.StoreAsync(storedLayout).WithCurrentCulture();

            return Result.AsSuccess();
        }
    }
}
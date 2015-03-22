using System.Collections.Generic;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Layout
{
    public class SaveLayoutCommand : ICommand<object>
    {
        public SaveLayoutCommand(string layoutName, string userId, Dictionary<string, byte[]> layoutData)
        {
            Guard.AgainstNullArgument("layoutName", layoutName);
            Guard.AgainstNullArgument("userId", userId);
            Guard.AgainstNullArgument("layoutData", layoutData);

            this.LayoutName = layoutName;
            this.UserId = userId;
            this.LayoutData = layoutData;
        }

        public string LayoutName { get; private set; }
        public string UserId { get; private set; }      
        public Dictionary<string, byte[]> LayoutData { get; private set; }
    }

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
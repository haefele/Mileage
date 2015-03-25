using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Layout
{
    public class GetLayoutCommand : ICommand<Dictionary<string, byte[]>>
    {
        public GetLayoutCommand([NotNull]string userId, [NotNull]string layoutName)
        {
            Guard.AgainstNullArgument("userId", userId);
            Guard.AgainstNullArgument("layoutName", layoutName);

            this.UserId = userId;
            this.LayoutName = layoutName;
        }

        public string UserId { get; private set; }
        public string LayoutName { get; private set; }
    }

    public class GetLayoutCommandHandler : CommandHandler<GetLayoutCommand, Dictionary<string, byte[]>>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetLayoutCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }

        public override async Task<Result<Dictionary<string, byte[]>>> Execute(GetLayoutCommand command, ICommandScope scope)
        {
            StoredLayout layout = await this._documentSession.LoadAsync<StoredLayout>(StoredLayout.CreateId(command.UserId, command.LayoutName)).WithCurrentCulture();

            if (layout == null)
                return Result.AsError(CommandMessages.LayoutNotFound);

            return Result.AsSuccess(layout.LayoutData);
        }
    }
}
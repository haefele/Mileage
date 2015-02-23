using System.Threading.Tasks;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class GetMileageSettingsCommand : ICommand<MileageSettings>
    {
         
    }

    public class GetMileageSettingsCommandHandler : CommandHandler<GetMileageSettingsCommand, MileageSettings>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetMileageSettingsCommandHandler(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }

        public override async Task<Result<MileageSettings>> Execute(GetMileageSettingsCommand command, ICommandScope scope)
        {
            MileageSettings settings = await this._documentSession.LoadAsync<MileageSettings>(MileageSettings.CreateId()).WithCurrentCulture();

            if (settings == null)
            {
                settings = new MileageSettings();
                await this._documentSession.StoreAsync(settings).WithCurrentCulture();
            }

            return Result.AsSuccess(settings);
        }
    }
}
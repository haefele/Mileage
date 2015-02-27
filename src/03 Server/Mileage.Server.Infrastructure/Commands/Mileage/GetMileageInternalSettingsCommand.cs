using System.Threading.Tasks;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class GetMileageInternalSettingsCommand : ICommand<MileageInternalSettings>
    {
         
    }

    public class GetMileageInternalSettingsCommandHandler : CommandHandler<GetMileageInternalSettingsCommand, MileageInternalSettings>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetMileageInternalSettingsCommandHandler(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }

        public override async Task<Result<MileageInternalSettings>> Execute(GetMileageInternalSettingsCommand command, ICommandScope scope)
        {
            using(this._documentSession.Advanced.DocumentStore.AggressivelyCache())
            { 
                MileageInternalSettings settings = await this._documentSession.LoadAsync<MileageInternalSettings>(MileageInternalSettings.CreateId()).WithCurrentCulture();

                if (settings == null)
                {
                    settings = new MileageInternalSettings();
                    await this._documentSession.StoreAsync(settings).WithCurrentCulture();
                }

                return Result.AsSuccess(settings);
            }
        }
    }
}
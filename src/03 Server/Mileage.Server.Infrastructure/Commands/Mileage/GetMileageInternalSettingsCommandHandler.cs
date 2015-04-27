using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Mileage;
using Mileage.Shared.Entities.Mileage;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class GetMileageInternalSettingsCommandHandler : ICommandHandler<GetMileageInternalSettingsCommand, MileageInternalSettings>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMileageInternalSettingsCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public GetMileageInternalSettingsCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<Result<MileageInternalSettings>> Execute(GetMileageInternalSettingsCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument("command", command);
            Guard.AgainstNullArgument("scope", scope);

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
        #endregion
    }
}
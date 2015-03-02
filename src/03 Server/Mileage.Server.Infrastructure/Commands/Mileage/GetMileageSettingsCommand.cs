using System.Threading.Tasks;
using LiteGuard;
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
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMileageSettingsCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public GetMileageSettingsCommandHandler(IAsyncDocumentSession documentSession)
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
        public override async Task<Result<MileageSettings>> Execute(GetMileageSettingsCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument("command", command);
            Guard.AgainstNullArgument("scope", scope);

            using(this._documentSession.Advanced.DocumentStore.AggressivelyCache())
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
        #endregion
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities.Drivers;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Drivers
{
    public class SearchDriversCommand : ICommand<SearchDriversResult>
    {
        public SearchDriversCommand([CanBeNull]string searchText, [CanBeNull]string driversLicense, [CanBeNull]bool? personCarriageLicense, int skip, int take)
        {
            this.SearchText = searchText;
            this.DriversLicense = driversLicense;
            this.PersonCarriageLicense = personCarriageLicense;
            this.Skip = skip;
            this.Take = take;
        }

        public string SearchText { get; private set; }
        public string DriversLicense { get; private set; }
        public bool? PersonCarriageLicense { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
    }

    public class SearchDriversCommandHandler : CommandHandler<SearchDriversCommand, SearchDriversResult>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public SearchDriversCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }

        public override async Task<Result<SearchDriversResult>> Execute(SearchDriversCommand command, ICommandScope scope)
        {
            IAsyncDocumentQuery<DriversForSearch.Result> query = this._documentSession.Advanced.AsyncDocumentQuery<DriversForSearch.Result, DriversForSearch>();

            if (command.SearchText != null)
                query.Search(f => f.SearchText, command.SearchText);

            if (command.DriversLicense != null)
                query.WhereEquals(f => f.DriversLicenses, command.DriversLicense);

            IList<DriversForSearch.Result> result = await query.ToListAsync();
            return Result.AsError("asdf");
        }
    }
}
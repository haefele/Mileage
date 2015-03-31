using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities.Drivers;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;

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
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDriversCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public SearchDriversCommandHandler(IAsyncDocumentSession documentSession)
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
        public override async Task<Result<SearchDriversResult>> Execute(SearchDriversCommand command, ICommandScope scope)
        {
            IAsyncDocumentQuery<Driver> query = this._documentSession.Advanced.AsyncDocumentQuery<Driver, DriversForSearch>();
            
            if (command.SearchText != null)
                query.Search((DriversForSearch.Result f) => f.SearchText, command.SearchText);

            if (command.DriversLicense != null)
                query.WhereEquals((DriversForSearch.Result f) => f.DriversLicenses, command.DriversLicense);

            if (command.PersonCarriageLicense != null)
                query.WhereEquals((DriversForSearch.Result f) => f.PersonCarriageLicense, command.PersonCarriageLicense.Value);

            IList<Driver> result = await query
                .Skip(command.Skip)
                .Take(command.Take)
                .ToListAsync()
                .WithCurrentCulture();

            if (result.Any())
                return Result.AsSuccess(SearchDriversResult.WithDrivers(result));

            SuggestionQueryResult suggestions = await this._documentSession.Query<DriversForSearch.Result, DriversForSearch>()
                .Search(f => f.SearchTextForSuggestions, command.SearchText)
                .SuggestAsync()
                .WithCurrentCulture();
            
            if (suggestions.Suggestions.Any())
                return Result.AsSuccess(SearchDriversResult.WithSuggestions(suggestions.Suggestions));

            return Result.AsSuccess(SearchDriversResult.WithNoResults());
        }
        #endregion
    }
}
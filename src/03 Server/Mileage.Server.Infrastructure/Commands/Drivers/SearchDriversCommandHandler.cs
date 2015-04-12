using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Drivers;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Entities.Drivers;
using Mileage.Shared.Entities.Search;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;

namespace Mileage.Server.Infrastructure.Commands.Drivers
{
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
            IList<Driver> result = await this
                .ExecuteActualQueryAsync(command.SearchText, command.Skip, command.Take)
                .WithCurrentCulture();

            if (result.Any())
                return Result.AsSuccess(SearchDriversResult.WithDrivers(result));

            SuggestionQueryResult suggestions = await this._documentSession.Query<DocumentsForSearch.Result, DocumentsForSearch>()
                .Search(f => f.SearchText, command.SearchText)
                .SuggestAsync()
                .WithCurrentCulture();

            if (suggestions.Suggestions.Count() == 1)
            {
                result = await this
                    .ExecuteActualQueryAsync(suggestions.Suggestions.First(), command.Skip, command.Take)
                    .WithCurrentCulture();

                return Result.AsSuccess(SearchDriversResult.WithDrivers(result));
            }
            
            if (suggestions.Suggestions.Any())
                return Result.AsSuccess(SearchDriversResult.WithSuggestions(suggestions.Suggestions));

            return Result.AsSuccess(SearchDriversResult.WithNoResults());
        }
        #endregion

        #region Private Methods
        private Task<IList<Driver>> ExecuteActualQueryAsync(string searchText, int skip, int take)
        {
            var query = this._documentSession.Advanced.AsyncDocumentQuery<Driver, DocumentsForSearch>()
                .WhereEquals((DocumentsForSearch.Result f) => f.Item, SearchableItem.Driver);

            if (searchText != null)
                query.Search((DocumentsForSearch.Result f) => f.SearchText, searchText);

            return query
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        #endregion
    }
}
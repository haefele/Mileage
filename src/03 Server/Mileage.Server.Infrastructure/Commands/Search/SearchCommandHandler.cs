﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.NLog;
using LiteGuard;
using Metrics;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Drivers;
using Mileage.Server.Contracts.Commands.Search;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Common;
using Mileage.Shared.Extensions;
using Mileage.Shared.Models;
using Mileage.Shared.Results;
using Raven.Abstractions.Data;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Search
{
    public class SearchCommandHandler : ICommandHandler<SearchCommand, SearchResult>
    {
        #region Fields
        private readonly Timer _searchTimer = Metric.Timer("Search", Unit.Calls);

        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public SearchCommandHandler(IAsyncDocumentSession documentSession)
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
        public async Task<Result<SearchResult>> Execute(SearchCommand command, ICommandScope scope)
        {
            using(_searchTimer.NewContext(command.SearchText))
            {
                var result = await this
                    .ExecuteActualQueryAsync(command.SearchText, command.Skip, command.Take)
                    .WithCurrentCulture();

                //If we find results, we can just return them
                if (result.Any())
                    return Result.AsSuccess(SearchResult.WithItems(result));
            
                //If we find no results, we can suggest spelling corrections to the user
                SuggestionQueryResult suggestions = await this._documentSession.Query<DocumentsForSearch.Result, DocumentsForSearch>()
                    .Search(f => f.SearchText, command.SearchText)
                    .SuggestAsync()
                    .WithCurrentCulture();

                //If we get only one suggestion, we can just search for the suggested text
                //This way, if the user makes a minor spelling error, we have automatically corrected him
                if (suggestions.Suggestions.Count() == 1)
                {
                    result = await this
                        .ExecuteActualQueryAsync(suggestions.Suggestions.First(), command.Skip, command.Take)
                        .WithCurrentCulture();

                    if (result.Any())
                        return Result.AsSuccess(SearchResult.WithItemsThroughSuggestion(suggestions.Suggestions.First(), result));
                }

                //Return the suggestions if any
                if (suggestions.Suggestions.Any())
                    return Result.AsSuccess(SearchResult.WithSuggestions(suggestions.Suggestions));

                return Result.AsSuccess(SearchResult.WithNoResults());
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes the actual query asynchronously.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        private async Task<List<SearchItem>> ExecuteActualQueryAsync(string searchText, int skip, int take)
        {
            var query = this._documentSession.Advanced.AsyncDocumentQuery<object, DocumentsForSearch>();
            
            if (searchText != null)
                query.Search((DocumentsForSearch.Result f) => f.SearchText, searchText + "*", EscapeQueryOptions.AllowPostfixWildcard);

            FieldHighlightings highlightings;

            var result = await query
                .Highlight((DocumentsForSearch.Result f) => f.SearchText, 128, 1, out highlightings)
                .SetHighlighterTags(Highlightings.StartTag, Highlightings.EndTag)
                .SelectFields<DocumentsForSearch.Result>()
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .WithCurrentCulture();

            return result
                .Select(f => new SearchItem(f.Id, f.DisplayName, f.Item, highlightings.GetFragments(f.Id).FirstOrDefault()))
                .ToList();
        }
        #endregion
    }
}
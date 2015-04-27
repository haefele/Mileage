using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Search;
using Mileage.Server.Infrastructure.Raven.Indexes;
using Mileage.Shared.Extensions;
using Mileage.Shared.Models;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Search
{
    public class GetTopTagsWithCountsCommandHandler : ICommandHandler<GetTopTagsWithCountsCommand, IList<TagWithCount>>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopTagsWithCountsCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public GetTopTagsWithCountsCommandHandler(IAsyncDocumentSession documentSession)
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
        public async Task<Result<IList<TagWithCount>>> Execute(GetTopTagsWithCountsCommand command, ICommandScope scope)
        {
            IList<DocumentsWithTags.Result> tags = await this._documentSession.Advanced
                .AsyncDocumentQuery<DocumentsWithTags.Result, DocumentsWithTags>()
                .OrderByDescending(f => f.Count)
                .Take(command.CountOfTags)
                .ToListAsync()
                .WithCurrentCulture();

            IList<TagWithCount> convertedTags = tags
                .Select(f => new TagWithCount(f.Tag, f.Count))
                .ToList();

            return Result.AsSuccess(convertedTags);
        }
        #endregion
    }
}
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Search;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Search")]
    public class SearchController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public SearchController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }
        #endregion

        [HttpGet]
        [MileageAuthentication]
        [Route]
        public async Task<HttpResponseMessage> Search(string searchText = null)
        {
            Result<SearchResult> result = await this.CommandExecutor
                .Execute(new SearchCommand(
                    searchText,
                    this.Paging.Skip,
                    this.Paging.Take))
                .WithCurrentCulture();

            if (result.IsError)
                return this.Request.GetMessageWithError(HttpStatusCode.InternalServerError, result.Message);

            switch (result.Data.Status)
            {
                case SearchResultStatus.Found:
                    return this.Request.GetMessageWithObject(HttpStatusCode.Found, result.Data.Items);
                case SearchResultStatus.Suggestions:
                    return this.Request.GetMessageWithObject(HttpStatusCode.SeeOther, result.Data.Suggestions);
                default:
                    return this.Request.GetMessage(HttpStatusCode.NotFound);
            }
        }
    }
}
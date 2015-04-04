using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Drivers;
using Mileage.Server.Contracts.Commands.Search;
using Mileage.Server.Infrastructure.Commands.Drivers;
using Mileage.Server.Infrastructure.Commands.Layout;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class TestsController : BaseController
    {
        public TestsController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }

        [HttpGet]
        [Route("Tests")]
        public async Task<HttpResponseMessage> GetTests(string searchText = null)
        {
            Result<SearchResult> result = await this.CommandExecutor.Execute(new SearchCommand(searchText, this.Paging.Skip, this.Paging.Take)).WithCurrentCulture();

            if (result.IsError)
                return this.Request.GetMessageWithError(HttpStatusCode.InternalServerError, result.Message);

            if (result.Data.Status == SearchResultStatus.Found)
                return this.Request.GetMessageWithObject(HttpStatusCode.Found, result.Data.Items);
            else
                return this.Request.GetMessageWithObject(HttpStatusCode.SeeOther, result.Data.Suggestions);
        }
    }
}

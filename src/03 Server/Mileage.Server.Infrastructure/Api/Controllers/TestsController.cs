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
using Mileage.Server.Infrastructure.Commands.Layout;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class TestsController : BaseController
    {
        private readonly IDocumentStore _documentStore;

        public TestsController(ICommandExecutor commandExecutor, IDocumentStore documentStore) : base(commandExecutor)
        {
            _documentStore = documentStore;
        }

        [HttpGet]
        [Route("Tests")]
        public async Task<HttpResponseMessage> GetTests()
        {
            var layout = new StoredLayout
            {
                UserId = "users/1",
                LayoutName = "TestLayout"
            };
            layout.Id = StoredLayout.CreateId(layout.UserId, layout.LayoutName);

            using (var session = this._documentStore.OpenAsyncSession())
            {

                await session.StoreAsync(layout);
                await session.SaveChangesAsync();
            }

            using (var session = this._documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(layout);
                await session.SaveChangesAsync();
            }

            //Result<object> result = await this.CommandExecutor.Execute(new SaveLayoutCommand(layout));

            //result = await this.CommandExecutor.Execute(new SaveLayoutCommand(layout));

            //return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result, ignoreData: true);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

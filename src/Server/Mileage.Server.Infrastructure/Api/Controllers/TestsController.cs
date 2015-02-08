using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class TestsController : BaseController
    {
        public TestsController(IAsyncDocumentSession documentSession, IAsyncFilesSession filesSession) 
            : base(documentSession, filesSession)
        {
        }

        [HttpGet]
        [Route("Tests")]
        public async Task<HttpResponseMessage> GetTests()
        {
            return this.Request.GetMessageWithObject(HttpStatusCode.OK, new {Message = "asdf"});
        }
    }
}

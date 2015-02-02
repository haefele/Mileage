using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
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

        [Route("Tests")]
        [HttpGet]
        public HttpResponseMessage GetTests()
        {
            return this.GetMessageWithObject(HttpStatusCode.OK, new {Data = "asdf"});
        }
    }
}

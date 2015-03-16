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
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public class TestsController : BaseController
    {
        public TestsController(ICommandExecutor commandExecutor) : base(commandExecutor)
        {
        }

        [HttpGet]
        [Route("Tests")]
        public HttpResponseMessage GetTests()
        {
            throw new Exception("yolo wolo");
            //return this.Request.GetMessageWithObject(HttpStatusCode.OK, new {Message = "asdf"});
        }
    }
}

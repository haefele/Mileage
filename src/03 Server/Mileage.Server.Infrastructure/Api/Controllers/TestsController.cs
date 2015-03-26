﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands.Drivers;
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
        public TestsController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }

        [HttpGet]
        [Route("Tests")]
        public async Task<HttpResponseMessage> GetTests()
        {
            var result = await this.CommandExecutor.Execute(new SearchDriversCommand(null, null, null, 0, 10));
            return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result);
        }
    }
}

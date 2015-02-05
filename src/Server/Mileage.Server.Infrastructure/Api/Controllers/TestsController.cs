using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
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

        [Route("Tests")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTests()
        {
            var user = new User
            {
                Username = "haefele",
                IsDeactivated = false,
                NotificationEmailAddress = "haefele@xemio.net",
                PreferredLanguage = "de-DE"
            };
            await this.DocumentSession.StoreAsync(user);
            var authenticationData = new AuthenticationData
            {
                UserId = user.Id,
            };
            await this.DocumentSession.StoreAsync(authenticationData);

            return this.GetMessageWithObject(HttpStatusCode.OK, user);
        }
    }
}

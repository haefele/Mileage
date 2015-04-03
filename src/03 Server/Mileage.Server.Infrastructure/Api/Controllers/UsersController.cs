using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Users;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Commands.Users;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public UsersController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the currently authenticated <see cref="User"/>.
        /// </summary>
        /// <returns>
        /// 302 - Found: The user was found.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [Route("Me")]
        [HttpGet]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> GetMe()
        {
            Result<User> result = await this.CommandExecutor
                .Execute(new GetCurrentUserCommand())
                .WithCurrentCulture();
            return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.InternalServerError, result);
        }
        #endregion
    }
}
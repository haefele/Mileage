using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands.Mileage;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Models;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("auto")]
    public class AdminController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public AdminController(ICommandExecutor commandExecutor) 
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the admin user.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// 201 - Created: The admin user was created.
        /// 400 - BadRequest: Required data are missing.
        /// 409 - Conflict: 
        /// </returns>
        [HttpPost]
        [Route("a")]
        public async Task<HttpResponseMessage> CreateAdminUser(CreateAdminUserData data)
        {
            if (data == null || data.Username == null || data.EmailAddress == null || data.PasswordMD5Hash == null || data.Language == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, "asdf");

            Result<User> result = await this.CommandExecutor.Execute(new CreateAdminUserCommand(
                data.Username, 
                data.PasswordMD5Hash,
                data.EmailAddress, 
                data.Language));

            if (result.IsError)
                return this.Request.GetMessageWithResult(HttpStatusCode.Conflict, result);

            return this.Request.GetMessageWithObject(HttpStatusCode.Created, result.Data);
        }
        #endregion
    }
}
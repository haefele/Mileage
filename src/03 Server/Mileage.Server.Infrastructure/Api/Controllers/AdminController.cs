using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Localization.Server.Controllers;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Commands.Mileage;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Models;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Admin")]
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
        /// This works only once.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// 201 - Created: The admin user was created.
        /// 400 - BadRequest: Required data are missing.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("AdminUser")]
        public async Task<HttpResponseMessage> CreateAdminUser(CreateAdminUserData data)
        {
            if (data == null || data.EmailAddress == null || data.PasswordMD5Hash == null || data.Language == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ControllerMessages.RequiredDataAreMissing);
            
            Result<User> result = await this.CommandExecutor.Execute(new CreateAdminUserCommand(
                data.EmailAddress, 
                data.PasswordMD5Hash,
                data.Language));

            return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, result);
        }
        /// <summary>
        /// Makes sure that all indexes in the RavenDB database are created.
        /// </summary>
        /// <returns>
        /// 201 - Created: All indexes were created successfully.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("Indexes")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> CreateIndexes()
        {
            Result<object> result = await this.CommandExecutor.Execute(new CreateIndexesCommand());
            return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, result, ignoreData:true);
        }
        /// <summary>
        /// Resets all indexes in the RavenDB database.
        /// </summary>
        /// <returns>
        /// 200 - OK: All indexes were reset.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("Indexes/Reset")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> ResetIndexes()
        {
            var result = await this.CommandExecutor.Execute(new ResetIndexesCommand());
            return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result, ignoreData: true);
        }
        #endregion
    }
}
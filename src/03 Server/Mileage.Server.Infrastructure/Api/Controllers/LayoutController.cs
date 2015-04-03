using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Localization.Server.Commands;
using Mileage.Localization.Server.Controllers;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Layout;
using Mileage.Server.Contracts.Commands.Users;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Commands.Layout;
using Mileage.Server.Infrastructure.Commands.Users;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Layout")]
    public class LayoutController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public LayoutController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves the specified <paramref name="layoutData"/> with the specified <paramref name="layoutName"/>.
        /// </summary>
        /// <param name="layoutName">The name of the layout.</param>
        /// <param name="layoutData">The layout data.</param>
        /// <returns>
        /// 201 - Created: The layout was saved.
        /// 400 - BadRequest: Required data are missing.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("{layoutName}")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> SaveLayout(string layoutName, Dictionary<string, byte[]> layoutData)
        {
            if (layoutName == null || layoutData == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ControllerMessages.RequiredDataAreMissing);

            return await this.CommandExecutor.Batch(async scope =>
            {
                Result<User> currentUserResult = await scope
                    .Execute(new GetCurrentUserCommand())
                    .WithCurrentCulture();

                if (currentUserResult.IsError)
                    return this.Request.GetMessageWithError(HttpStatusCode.InternalServerError, currentUserResult.Message);

                Result<object> saveLayoutResult = await scope
                    .Execute(new SaveLayoutCommand(
                        layoutName, 
                        currentUserResult.Data.Id, 
                        layoutData))
                    .WithCurrentCulture();

                return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, saveLayoutResult, ignoreData: true);
            }).WithCurrentCulture();
        }
        /// <summary>
        /// Returns the layout with the specified <paramref name="layoutName"/>.
        /// </summary>
        /// <param name="layoutName">Name of the layout.</param>
        /// <returns>
        /// 302 - Found: The layout was found.
        /// 400 - BadRequest: Required data are missing.
        /// 404 - NotFound: The layout was not found.
        /// </returns>
        [HttpGet]
        [Route("{layoutName}")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> GetLayout(string layoutName)
        {
            if (layoutName == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ControllerMessages.RequiredDataAreMissing);

            return await this.CommandExecutor.Batch(async scope =>
            {
                Result<User> currentUserResult = await scope
                    .Execute(new GetCurrentUserCommand())
                    .WithCurrentCulture();

                if (currentUserResult.IsError)
                    return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.InternalServerError, currentUserResult, ignoreData: true);

                Result<Dictionary<string, byte[]>> layoutResult = await scope
                    .Execute(new GetLayoutCommand(
                        currentUserResult.Data.Id, 
                        layoutName))
                    .WithCurrentCulture();

                return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.NotFound, layoutResult);
            }).WithCurrentCulture();
        }
        #endregion
    }
}
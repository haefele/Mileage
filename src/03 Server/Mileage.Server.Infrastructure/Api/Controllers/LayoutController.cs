﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
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
        /// Saves the layout.
        /// </summary>
        /// <param name="layoutName">The name of the layout.</param>
        /// <param name="layoutData">The layout data.</param>
        /// <returns>
        /// 201 - Created: The layout was saved.
        /// 400 - BadRequest: Required data are missing.
        /// 500 - InternalServerError: An error occured.
        /// </returns>
        [HttpPost]
        [Route("Layout/{layoutName}")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> SaveLayout(string layoutName, Dictionary<string, byte[]> layoutData)
        {
            if (layoutName == null || layoutData == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, CommandMessages.AdminUserAlreadyCreated);

            return await this.CommandExecutor.Batch(async scope =>
            {
                Result<User> currentUserResult = await scope.Execute(new GetCurrentUserCommand()).WithCurrentCulture();

                if (currentUserResult.IsError)
                    return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.NotFound, currentUserResult);

                Result<object> saveLayoutResult = await scope.Execute(new SaveLayoutCommand(layoutName, currentUserResult.Data.Id, layoutData));
                return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, saveLayoutResult, ignoreData: true);
            }).WithCurrentCulture();
        }

        [HttpGet]
        [Route("Layout/{layoutName}")]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> GetLayout(string layoutName)
        {
            if (layoutName == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, CommandMessages.AdminUserAlreadyCreated);

            return await this.CommandExecutor.Batch(async scope =>
            {
                Result<User> currentUserResult = await scope.Execute(new GetCurrentUserCommand()).WithCurrentCulture();

                if (currentUserResult.IsError)
                    return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.InternalServerError, currentUserResult, ignoreData: true);

                Result<Dictionary<string, byte[]>> layoutResult = await scope.Execute(new GetLayoutCommand(currentUserResult.Data.Id, layoutName)).WithCurrentCulture();
                return this.Request.GetMessageWithResult(HttpStatusCode.Found, HttpStatusCode.NotFound, layoutResult);
            }).WithCurrentCulture();
        }
        #endregion
    }
}
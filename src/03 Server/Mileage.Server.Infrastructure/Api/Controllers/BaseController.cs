using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using LiteGuard;
using Microsoft.Owin;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;
using Raven.Database.FileSystem.Storage.Voron.Impl;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        #region Properties
        /// <summary>
        /// Gets the command executor.
        /// </summary>
        public ICommandExecutor CommandExecutor { get; private set; }
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets the owin context.
        /// </summary>
        public IOwinContext OwinContext
        {
            get { return this.Request.GetOwinContext(); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        protected BaseController(ICommandExecutor commandExecutor)
        {
            Guard.AgainstNullArgument("commandExecutor", commandExecutor);

            this.CommandExecutor = commandExecutor;

            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Overrides of ApiController
        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.ExecuteAsync(controllerContext, cancellationToken);

            return response;
        }
        #endregion
    }
}
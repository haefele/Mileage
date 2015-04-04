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
using Mileage.Server.Infrastructure.Api.Data;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;
using Raven.Database.FileSystem.Storage.Voron.Impl;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        #region Constants
        public const int DefaultSkip = 0;
        public const int DefaultTake = 50;
        #endregion

        #region Fields
        private PagingInfo _paging;
        #endregion

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
        /// <summary>
        /// Returns the paging informations.
        /// </summary>
        public PagingInfo Paging
        {
            get
            {
                if (this._paging != null)
                    return this._paging;

                string skipString = this.OwinContext.Request.Query.Get("skip");

                int skip;
                if (int.TryParse(skipString, out skip) == false)
                    skip = DefaultSkip;
                
                string takeString = this.OwinContext.Request.Query.Get("take");

                int take;
                if (int.TryParse(takeString, out take) == false)
                    take = DefaultTake;

                return this._paging = new PagingInfo(skip, take);
            }
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
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using LiteGuard;
using Microsoft.Owin;
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
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore
        {
            get { return this.DocumentSession.Advanced.DocumentStore; }
        }
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IAsyncDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets the files store.
        /// </summary>
        public IFilesStore FilesStore
        {
            get { return this.FilesSession.Advanced.FilesStore; }
        }
        /// <summary>
        /// Gets the files session.
        /// </summary>
        public IAsyncFilesSession FilesSession { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether an exception occured.
        /// This property gets set by the <see cref="HandleBusinessExceptionFilterAttribute"/>.
        /// </summary>
        public bool ExceptionOccured { get; internal set; }
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
        /// <param name="documentSession">The document session.</param>
        /// <param name="filesSession">The files session.</param>
        protected BaseController(IAsyncDocumentSession documentSession, IAsyncFilesSession filesSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this.DocumentSession = documentSession;
            this.FilesSession = filesSession;

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

            using (this.DocumentSession)
            {
                if (this.ExceptionOccured == false)
                {
                    await this.DocumentSession.SaveChangesAsync(cancellationToken);
                    await this.FilesSession.SaveChangesAsync();
                }
            }

            return response;
        }
        #endregion

        #region Methods
        public virtual HttpResponseMessage GetMessageWithError(HttpStatusCode statusCode, string message)
        {
            return this.Request.CreateErrorResponse(statusCode, new HttpError(message));
        }
        public virtual HttpResponseMessage GetMessageWithObject<T>(HttpStatusCode statusCode, T obj)
        {
            return this.Request.CreateResponse(statusCode, obj);
        }
        public virtual HttpResponseMessage GetMessageWithResult(HttpStatusCode statusCode, Result result)
        {
            return this.Request.CreateErrorResponse(statusCode, new HttpError(result.Message));
        }
        #endregion
    }
}
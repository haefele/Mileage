using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Castle.Core.Logging;
using Mileage.Server.Infrastructure.Api.Controllers;
using Mileage.Server.Infrastructure.Exceptions;
using Mileage.Server.Infrastructure.Extensions;

namespace Mileage.Server.Infrastructure.Api.Filters
{
    public class HandleBusinessExceptionFilterAttribute : ExceptionFilterAttribute
    {
        #region Overrides of ExceptionFilterAttribute
        /// <summary>
        /// Executed when an exception occured.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            ILogger logger = this.GetLogger(context);
            logger.Error("Exception occured.", context.Exception);

            if (context.ActionContext.ControllerContext.Controller is BaseController)
            {
                var controller = (BaseController)context.ActionContext.ControllerContext.Controller;
                controller.ExceptionOccured = true;
            }

            HttpError error = this.GetError(context.Exception);
            HttpStatusCode statusCode = this.GetStatusCode(context.Exception);

            context.Response = context.Request.CreateErrorResponse(statusCode, error);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        private ILogger GetLogger(HttpActionExecutedContext actionExecutedContext)
        {
            var loggerFactory = actionExecutedContext.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<ILoggerFactory>();
            Type loggerType = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType;

            return loggerFactory.Create(loggerType);
        }
        /// <summary>
        /// Creates the error for the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private HttpError GetError(Exception exception)
        {
            if (exception is BusinessException)
            {
                var businessException = (BusinessException)exception;

                var error = new HttpError(exception.Message);

                if (businessException.CustomResponse != null)
                    error.Add("AdditionalData", businessException.CustomResponse);

                return error;
            }
            else
            {
#if DEBUG
                return new HttpError(exception.ToString());
#else
                return new HttpError(FilterMessages.InternalServerError);
#endif
            }
        }
        /// <summary>
        /// Creates the status code for the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private HttpStatusCode GetStatusCode(Exception exception)
        {
            if (exception is BusinessException)
            {
                var businessException = (BusinessException)exception;
                return businessException.StatusCode;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        #endregion
    }
}
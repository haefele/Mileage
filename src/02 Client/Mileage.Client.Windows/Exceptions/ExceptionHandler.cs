using System;
using System.Web.Http;
using System.Windows;
using Castle.Core.Logging;
using LiteGuard;
using Mileage.Client.Contracts.Exceptions;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Windows.Messages;
using Mileage.Localization.Common;

namespace Mileage.Client.Windows.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        #region Fields
        private readonly IMessageService _messageService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="messageService">The message service.</param>
        public ExceptionHandler(IMessageService messageService)
        {
            Guard.AgainstNullArgument("messageService", messageService);

            this._messageService = messageService;

            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void Handle(HttpError error)
        {
            var exception = new KnownException(error.Message);
            this.Handle(exception);
        }
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Handle(Exception exception)
        {
            this.Logger.Error("Exception occured.", exception);

            if (exception is KnownException)
            {
                this.Logger.Debug("The exception is a KnownException, just showing the message to the user.");

                this._messageService.ShowConfirmationDialog(exception.Message, CommonMessages.Error, MessageImage.Information);
            }
            else
            {
                string message = string.Format("{0}{1}{2}", CommonMessages.UnhandledException, Environment.NewLine, exception.Message);
                this._messageService.ShowConfirmationDialog(message, CommonMessages.Error, MessageImage.Information);
            }
        }
        #endregion
    }
}
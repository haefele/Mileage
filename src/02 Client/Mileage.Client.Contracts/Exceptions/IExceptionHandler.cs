using System;
using System.Web.Http;

namespace Mileage.Client.Contracts.Exceptions
{
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        void Handle(HttpError error);
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void Handle(Exception exception); 
    }
}
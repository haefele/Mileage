using System;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Mileage.Server.Infrastructure.Exceptions
{
    [Serializable]
    public abstract class BusinessException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        protected BusinessException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected BusinessException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        protected BusinessException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public abstract HttpStatusCode StatusCode { get; }
        /// <summary>
        /// Gets or sets the custom response.
        /// </summary>
        public JObject CustomResponse { get; set; }
        #endregion
    }
}
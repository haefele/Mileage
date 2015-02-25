using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage GetMessageWithError(this HttpRequestMessage request, HttpStatusCode statusCode, string message)
        {
            return request.CreateErrorResponse(statusCode, new HttpError(message));
        }
        public static HttpResponseMessage GetMessageWithResult<T>(this HttpRequestMessage request, HttpStatusCode successStatusCode, HttpStatusCode errorStatusCode, Result<T> result, bool ignoreData = false)
        {
            if (result.IsError)
                return request.CreateErrorResponse(errorStatusCode, new HttpError(result.Message));
            else if (ignoreData)
                return request.CreateResponse(successStatusCode);
            else
                return request.CreateResponse(successStatusCode, result.Data);
        }
        public static HttpResponseMessage GetMessageWithObject<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T obj)
        {
            return request.CreateResponse(statusCode, obj);
        }

        public static HttpResponseMessage GetMessage(this HttpRequestMessage request, HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode);
        }
    }
}
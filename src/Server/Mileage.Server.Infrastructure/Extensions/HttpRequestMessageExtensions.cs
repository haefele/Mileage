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
        public static HttpResponseMessage GetMessageWithResult(this HttpRequestMessage request, HttpStatusCode statusCode, Result result)
        {
            return request.CreateErrorResponse(statusCode, new HttpError(result.Message));
        }
        public static HttpResponseMessage GetMessageWithResult<T>(this HttpRequestMessage request, HttpStatusCode statusCode, Result<T> result)
        {
            if (result.IsError)
                return request.CreateErrorResponse(statusCode, new HttpError(result.Message));
            else
                return request.CreateResponse(statusCode, result.Data);
        }
        public static HttpResponseMessage GetMessageWithObject<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T obj)
        {
            return request.CreateResponse(statusCode, obj);
        }
    }
}
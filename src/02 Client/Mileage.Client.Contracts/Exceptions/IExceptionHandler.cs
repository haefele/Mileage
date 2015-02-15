using System;
using System.Web.Http;

namespace Mileage.Client.Contracts.Exceptions
{
    public interface IExceptionHandler
    {
        void Handle(HttpError error);
        void Handle(Exception exception); 
    }
}
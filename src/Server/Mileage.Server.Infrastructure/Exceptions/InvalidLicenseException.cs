using System;
using System.Net;

namespace Mileage.Server.Infrastructure.Exceptions
{
    public class InvalidLicenseException : BusinessException
    {
        public override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.ServiceUnavailable; }
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Mileage.Server.Infrastructure.Api.Controllers;

namespace Mileage.Server.Infrastructure.Api.Configuration
{
    public class MileageAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly>
            {
                typeof(BaseController).Assembly
            };
        }
    }
}
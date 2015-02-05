using System.Web.Http.Dependencies;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class DependencyScopeExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="dependencyScope">The dependency scope.</param>
        public static T GetService<T>(this IDependencyScope dependencyScope)
        {
            return (T)dependencyScope.GetService(typeof(T));
        }
    }
}
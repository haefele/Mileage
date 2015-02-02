using System.Web.Http.Dependencies;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class DependencyResolverExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public static T GetService<T>(this IDependencyResolver dependencyResolver)
        {
            return (T)dependencyResolver.GetService(typeof(T));
        }
    }
}
using System.Web.Http.Dependencies;
using JetBrains.Annotations;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class DependencyScopeExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="dependencyScope">The dependency scope.</param>
        [CanBeNull]
        public static T GetService<T>([NotNull]this IDependencyScope dependencyScope)
        {
            return (T)dependencyScope.GetService(typeof(T));
        }
    }
}
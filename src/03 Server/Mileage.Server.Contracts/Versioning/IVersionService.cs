using System;
using JetBrains.Annotations;

namespace Mileage.Server.Contracts.Versioning
{
    public interface IVersionService : IService
    {
        /// <summary>
        /// Gets the current version.
        /// </summary>
        [NotNull]
        Version GetCurrentVersion();
    }
}
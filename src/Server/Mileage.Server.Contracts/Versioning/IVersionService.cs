using System;

namespace Mileage.Server.Contracts.Versioning
{
    public interface IVersionService
    {
        /// <summary>
        /// Gets the current version.
        /// </summary>
        Version GetCurrentVersion();
    }
}
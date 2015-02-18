using System;

namespace Mileage.Client.Contracts.Versioning
{
    public interface IVersionService
    {
        /// <summary>
        /// Gets the current version.
        /// </summary>
        Version GetCurrentVersion();
    }
}
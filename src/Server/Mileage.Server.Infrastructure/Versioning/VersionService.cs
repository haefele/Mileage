using System;
using System.Reflection;
using Mileage.Server.Contracts.Versioning;

namespace Mileage.Server.Infrastructure.Versioning
{
    public class VersionService : IVersionService
    {
        #region Fields
        private Version _currentVersion;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current version.
        /// </summary>
        public Version GetCurrentVersion()
        {
            return this._currentVersion ?? (this._currentVersion = Assembly.GetEntryAssembly().GetName().Version);
        }
        #endregion
    }
}
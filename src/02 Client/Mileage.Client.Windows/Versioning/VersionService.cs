using System;
using System.Reflection;
using Mileage.Client.Contracts.Versioning;

namespace Mileage.Client.Windows.Versioning
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
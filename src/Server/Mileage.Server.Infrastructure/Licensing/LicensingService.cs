using System;   
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.Core.Logging;
using LiteGuard;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Contracts.Versioning;
using Mileage.Server.Infrastructure.Exceptions;
using Mileage.Shared.Results;
using Portable.Licensing;
using Portable.Licensing.Validation;
using Simulated;

namespace Mileage.Server.Infrastructure.Licensing
{
    public class LicensingService : ILicensingService
    {
        #region Constants
        /// <summary>
        /// The public key of the license key-pair.
        /// </summary>
        private const string PublicKey = "";
        #endregion

        #region Fields
        private readonly IVersionService _versionService;
        private readonly FileSystem _fileSystem;

        private License _license;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicensingService"/> class.
        /// </summary>
        /// <param name="versionService">The version service.</param>
        /// <param name="fileSystem">The file system.</param>
        public LicensingService(IVersionService versionService, FileSystem fileSystem)
        {
            Guard.AgainstNullArgument("versionService", versionService);
            Guard.AgainstNullArgument("fileSystem", fileSystem);

            this._versionService = versionService;
            this._fileSystem = fileSystem;

            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the license from the specified <paramref name="licensePath" />.
        /// </summary>
        /// <param name="licensePath">The license path.</param>
        public Result LoadLicense(string licensePath)
        {
            Guard.AgainstNullArgument("licensePath", licensePath);

            if (this._fileSystem.File(licensePath).Exists == false)
            {
                this.Logger.WarnFormat("License not found. Path is: {0}", licensePath);
                return Result.AsError("License not found.");
            }

            Result<License> licenseResult = this.LoadLicenseFromFile(licensePath);

            if (licenseResult.IsError)
                return licenseResult;

            this._license = licenseResult.Data;

            return Result.AsSuccess();
        }
        /// <summary>
        /// Returns whether the license is valid and is valid for the specified <paramref name="clientId" />.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        public Result AssertValidLicense(string clientId)
        {
            Guard.AgainstNullArgument("clientId", clientId);

#if DEBUG
            this.Logger.Debug("Running in DEBUG mode, not validating the license.");
            return Result.AsSuccess();
#else

            if (this._license == null)
                return Result.AsError("No license has been loaded.");
            
            List<IValidationFailure> errors = this._license
                .Validate().ExpirationDate()
                .And().Signature(PublicKey)
                .And().AssertThat(LicenseIsValidForCurrentVersion, new LicenseIsInvalidForCurrentVersionValidationFailure())
                .And().AssertThat(f => this.LicenseHasClientId(f, clientId), new ClientIdMissingValidationFailure())
                .AssertValidLicense()
                .ToList();

            if (errors.Any())
            {
                this.Logger.ErrorFormat("The license is invalid. {0}", string.Join(", ", errors.Select(f => f.Message)));
                return Result.AsError("There are some errors with the license file. Take a look at the logs to get more information.");
            }

            return Result.AsSuccess();
#endif
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the license from the specified <paramref name="licensePath"/>.
        /// </summary>
        /// <param name="licensePath">The license path.</param>
        private Result<License> LoadLicenseFromFile(string licensePath)
        {
            Guard.AgainstNullArgument("licensePath", licensePath);

            try
            {
                using (FileStream stream = File.OpenRead(licensePath))
                {
                    return Result.AsSuccess(License.Load(stream));
                }
            }
            catch (Exception exception)
            {
                this.Logger.Error(string.Format("Exception while loading the license. Path is: {0}", licensePath), exception);
                return Result.AsError(exception.Message);
            }
        }
        /// <summary>
        /// Returns whether the license is valid with the current version of Mileage.
        /// </summary>
        /// <param name="license">The license.</param>
        private bool LicenseIsValidForCurrentVersion(License license)
        {
            return new Version(license.AdditionalAttributes.Get("Version")) <= this._versionService.GetCurrentVersion();
        }
        /// <summary>
        /// Returns whether the license is valid for the specified <paramref name="clientId"/>.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="clientId">The client identifier.</param>
        private bool LicenseHasClientId(License license, string clientId)
        {
            return license.ProductFeatures.Get("Products").Split(';').Contains(clientId);
        }
        #endregion
    }
}
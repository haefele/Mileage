using System;   
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using Castle.Core.Logging;
using LiteGuard;
using Mileage.Localization.Server.Licensing;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Contracts.Versioning;
using Mileage.Shared.Common;
using Mileage.Shared.Results;
using Portable.Licensing;
using Portable.Licensing.Validation;

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
        private readonly IFileSystem _fileSystem;

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
        public LicensingService(IVersionService versionService, IFileSystem fileSystem)
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

            if (string.IsNullOrWhiteSpace(licensePath))
            {
                this.Logger.WarnFormat("License path is empty.");
                return Result.AsError(LicensingMessages.LicenseNotFound);
            }

            if (this._fileSystem.FileInfo.FromFileName(licensePath).Exists == false)
            {
                this.Logger.WarnFormat("License not found. Path is: {0}", licensePath);
                return Result.AsError(LicensingMessages.LicenseNotFound);
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
                return Result.AsError(LicensingMessages.LicenseNotFound);

            if (ClientIds.Get().Any(f => string.Equals(f, clientId, StringComparison.InvariantCultureIgnoreCase)) == false)
                return Result.AsError(LicensingMessages.InvalidClient);

            List<IValidationFailure> errors = this._license
                .Validate().ExpirationDate()
                .And().Signature(PublicKey)
                .And().AssertThat(LicenseIsValidForCurrentVersion, new LicenseIsInvalidForCurrentVersionValidationFailure())
                .And().AssertThat(f => this.LicenseHasClientId(f, clientId), new ClientIdMissingValidationFailure())
                .AssertValidLicense()
                .ToList();

            if (errors.Any())
            {
                string licenseErrorMessages = this.GetLicenseErrorMessagesForClient(errors);

                this.Logger.ErrorFormat("The license is invalid. {0}", licenseErrorMessages);
                return Result.AsError(string.Format(LicensingMessages.LicenseIsInvalid, licenseErrorMessages));
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

            return Result.Create(() =>
            {
                using (Stream stream = this._fileSystem.File.OpenRead(licensePath))
                {
                    return License.Load(stream);
                }
            });
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
        /// <summary>
        /// Creates the license error messages for the client.
        /// </summary>
        /// <param name="errors">The errors.</param>
        private string GetLicenseErrorMessagesForClient(IEnumerable<IValidationFailure> errors)
        {
            return errors
                .Select(f =>
                    {
                        if (f is LicenseIsInvalidForCurrentVersionValidationFailure)
                            return LicensingMessages.LicenseIsInvalidForCurrentVersion;

                        if (f is ClientIdMissingValidationFailure)
                            return LicensingMessages.ClientIdMissing;

                        if (f is InvalidSignatureValidationFailure)
                            return LicensingMessages.InvalidSignature;

                        if (f is LicenseExpiredValidationFailure)
                            return LicensingMessages.LicenseExpired;

                        this.Logger.DebugFormat("Creating license error message for the client. Could not identify this validation failure: {0}", f.GetType());

                        return string.Empty;
                    })
                .Aggregate(string.Empty, (current, part) => current + " " + part);
        }
        #endregion
    }
}
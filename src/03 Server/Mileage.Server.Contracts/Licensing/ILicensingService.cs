using JetBrains.Annotations;
using Mileage.Shared.Licensing;
using Mileage.Shared.Results;

namespace Mileage.Server.Contracts.Licensing
{
    public interface ILicensingService : IService
    {
        /// <summary>
        /// Gets the license information.
        /// </summary>
        Result<LicenseInfo> GetLicenseInfo();

        /// <summary>
        /// Loads the license from the specified <paramref name="licensePath"/>.
        /// </summary>
        /// <param name="licensePath">The license path.</param>
        [NotNull]
        Result LoadLicense([NotNull]string licensePath);
        /// <summary>
        /// Returns whether the license is valid and is valid for the specified <paramref name="clientId"/>.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [NotNull]
        Result AssertValidLicense([NotNull]string clientId);
    }
}
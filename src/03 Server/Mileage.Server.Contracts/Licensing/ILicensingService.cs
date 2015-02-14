using Mileage.Shared.Results;

namespace Mileage.Server.Contracts.Licensing
{
    public interface ILicensingService : IService
    {
        /// <summary>
        /// Loads the license from the specified <paramref name="licensePath"/>.
        /// </summary>
        /// <param name="licensePath">The license path.</param>
        Result LoadLicense(string licensePath);
        /// <summary>
        /// Returns whether the license is valid and is valid for the specified <paramref name="clientId"/>.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        Result AssertValidLicense(string clientId);
    }
}
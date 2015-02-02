using Castle.Core;
using Castle.MicroKernel.Registration;
using LiteGuard;
using Mileage.Server.Contracts.Licensing;
using Mileage.Server.Infrastructure.Licensing;

namespace Mileage.Server.Infrastructure.Bootstrapper
{
    public class LoadLicenseStartable : IStartable
    {
        #region Fields
        private readonly ILicensingService _licensingService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadLicenseStartable"/> class.
        /// </summary>
        /// <param name="licensingService">The licensing service.</param>
        public LoadLicenseStartable(ILicensingService licensingService)
        {
            Guard.AgainstNullArgument("licensingService", licensingService);

            this._licensingService = licensingService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this._licensingService.LoadLicense(Dependency.OnAppSettingsValue("Mileage/LicensePath").Value);
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}
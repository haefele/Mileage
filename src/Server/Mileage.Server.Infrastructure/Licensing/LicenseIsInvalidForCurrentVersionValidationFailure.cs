using Portable.Licensing.Validation;

namespace Mileage.Server.Infrastructure.Licensing
{
    public class LicenseIsInvalidForCurrentVersionValidationFailure : IValidationFailure
    {
        public LicenseIsInvalidForCurrentVersionValidationFailure()
        {
            this.Message = "You don't have the license to use this version of mileage!";
            this.HowToResolve = "Contact your distributor/vendor to upgrade your license.";
        }

        public string Message { get; set; }
        public string HowToResolve { get; set; }
    }
}
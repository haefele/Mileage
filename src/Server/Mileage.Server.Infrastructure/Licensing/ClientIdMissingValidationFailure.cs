using Portable.Licensing.Validation;

namespace Mileage.Server.Infrastructure.Licensing
{
    public class ClientIdMissingValidationFailure : IValidationFailure
    {
        public ClientIdMissingValidationFailure()
        {
            this.Message = "You don't have the license to use this client!";
            this.HowToResolve = "Contact your distributor/vendor to buy the required license.";
        }

        public string Message { get; set; }
        public string HowToResolve { get; set; }
    }
}
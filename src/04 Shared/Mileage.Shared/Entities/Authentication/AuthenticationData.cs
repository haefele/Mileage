namespace Mileage.Shared.Entities.Authentication
{
    public class AuthenticationData : AggregateRoot
    {
        public static string CreateId(string userId)
        {
            return string.Format("{0}/AuthenticationData", userId);
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        public byte[] Salt { get; set; }
        /// <summary>
        /// Gets or sets the hash of the users password, combined with the salt.
        /// </summary>
        public byte[] Hash { get; set; }
    }
}
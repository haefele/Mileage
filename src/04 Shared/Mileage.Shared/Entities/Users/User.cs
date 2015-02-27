namespace Mileage.Shared.Entities.Users
{
    public class User : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the preferred language.
        /// (For example: en-US, de-DE)
        /// The <see cref="PreferredLanguage"/> is optional and not required.
        /// </summary>
        public string PreferredLanguage { get; set; }
        /// <summary>
        /// Gets or sets a whether this user is deactivated.
        /// </summary>
        public bool IsDeactivated { get; set; }
    }
}
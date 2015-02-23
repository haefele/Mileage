namespace Mileage.Shared.Entities.Users
{
    public class User : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the <see cref="Username"/>.
        /// The <see cref="Username"/> is able to change, but must be unique.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the notification email address.
        /// The <see cref="NotificationEmailAddress"/> is optional and not required.
        /// </summary>
        public string NotificationEmailAddress { get; set; }
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
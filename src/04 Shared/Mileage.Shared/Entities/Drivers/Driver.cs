using System;
using System.Collections.Generic;

namespace Mileage.Shared.Entities.Drivers
{
    public class Driver : AggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Driver"/> class.
        /// </summary>
        public Driver()
        {
            this.DriversLicenses = new List<DriversLicense>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        public DateTimeOffset Birthday { get; set; }
        /// <summary>
        /// Gets or sets the drivers licenses.
        /// </summary>
        public List<DriversLicense> DriversLicenses { get; set; }
        /// <summary>
        /// Gets or sets the date of the last drivers license check.
        /// (Letzte Führerscheinüberprüfung)
        /// </summary>
        public DateTimeOffset? LastDriversLicenseCheck { get; set; }
        /// <summary>
        /// Gets or sets the date of the last fitness to drive check.
        /// (Letzte Fahrtauglichkeitsüberprüfung)
        /// </summary>
        public DateTimeOffset? LastFitnessToDriveCheck { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this driver has the license to carry persons.
        /// (Beförderungslizenz)
        /// </summary>
        public bool PersonCarriageLicense { get; set; }
        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the date of entry.
        /// (Eintrittsdatum)
        /// </summary>
        public DateTimeOffset DateOfEntry { get; set; }
        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public Address Address { get; set; }
        #endregion
    }
}
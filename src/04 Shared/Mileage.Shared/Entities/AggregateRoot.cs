using System;

namespace Mileage.Shared.Entities
{
    public abstract class AggregateRoot : IEquatable<AggregateRoot>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Equals and GetHashCode
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(AggregateRoot other)
        {
            return string.Equals(Id, other.Id);
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj)) 
                return false;
            if (object.ReferenceEquals(this, obj)) 
                return true;

            if (obj.GetType() != this.GetType()) 
                return false;

            return Equals((AggregateRoot)obj);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
        #endregion
    }
}
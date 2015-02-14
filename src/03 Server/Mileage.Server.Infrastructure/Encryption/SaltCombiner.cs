using System.Security.Cryptography;
using LiteGuard;
using Mileage.Server.Contracts.Encryption;

namespace Mileage.Server.Infrastructure.Encryption
{
    public class SaltCombiner : ISaltCombiner
    {
        #region Implementation of ISaltCombiner
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        public byte[] Combine(byte[] salt, string password)
        {
            Guard.AgainstNullArgument("salt", salt);
            Guard.AgainstNullArgument("password", password);

            using (var hasher = new Rfc2898DeriveBytes(password, salt))
            {
                return hasher.GetBytes(128);
            }
        }
        #endregion
    }
}
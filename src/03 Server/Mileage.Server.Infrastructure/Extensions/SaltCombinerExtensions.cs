using System;
using Mileage.Server.Contracts.Encryption;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class SaltCombinerExtensions
    {
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        public static byte[] Combine(this ISaltCombiner saltCombiner, byte[] salt, byte[] password)
        {
            return saltCombiner.Combine(salt, BitConverter.ToString(password));
        }
    }
}
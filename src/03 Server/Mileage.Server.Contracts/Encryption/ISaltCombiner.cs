using JetBrains.Annotations;

namespace Mileage.Server.Contracts.Encryption
{
    public interface ISaltCombiner : IService
    {
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        [NotNull]
        byte[] Combine([NotNull]byte[] salt, [NotNull]string password);
    }
}
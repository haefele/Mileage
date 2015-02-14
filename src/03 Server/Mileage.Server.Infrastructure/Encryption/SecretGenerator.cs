using System.Security.Cryptography;
using Mileage.Server.Contracts.Encryption;

namespace Mileage.Server.Infrastructure.Encryption
{
    public class SecretGenerator : ISecretGenerator
    {
        #region Implementation of ISecretGenerator
        /// <summary>
        /// Generates a new secret.
        /// </summary>
        public byte[] Generate(int length = 128)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);

            return randomBytes;
        }
        #endregion
    }
}
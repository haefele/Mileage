using System;
using Mileage.Server.Contracts.Encryption;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class SecretGeneratorExtensions
    {
        public static string GenerateString(this ISecretGenerator generator, int length = 128)
        {
            byte[] randomData = generator.Generate((length / 4 * 3) + 1);
            return Convert.ToBase64String(randomData).Substring(0, length).Replace('/', '-').Replace('+', '_');
        }
    }
}
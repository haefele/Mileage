namespace Mileage.Server.Contracts.Encryption
{
    public interface ISecretGenerator : IService
    {
        /// <summary>
        /// Generates a new secret with the the specified <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The length.</param>
        byte[] Generate(int length = 128);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portable.Licensing;
using Portable.Licensing.Security.Cryptography;
using Portable.Licensing.Validation;

namespace Mileage.Tests.Licensing
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyGenerator keyGenerator = KeyGenerator.Create();
            KeyPair keyPair = keyGenerator.GenerateKeyPair();

            string privateKey = keyPair.ToEncryptedPrivateKeyString("123123");
            string publicKey = keyPair.ToPublicKeyString();

            var license = License.New()
                .As(LicenseType.Trial)
                .ExpiresAt(new DateTime(2014, 3, 31))
                .LicensedTo("Mileage Software GmbH", string.Empty)
                .WithProductFeatures(f => f.Add("Clients", "Desktop:123123;iOS:123"))
                .WithUniqueIdentifier(Guid.NewGuid())
                .CreateAndSignWithPrivateKey(privateKey, "123123");

            var valid = license.Validate()
                .ExpirationDate().When(f => f.Type == LicenseType.Trial)
                .And().Signature(publicKey)
                .AssertValidLicense();

            var clients = license.ProductFeatures.Get("Clients");

            var parsedClients = clients.Split(';').Select(f => new
            {
                Client = f.Split(':')[0],
                Secret = f.Split(':')[1]
            }).ToList();

            if (parsedClients.Any(f => f.Client == "Desktop" && f.Secret == "123123"))
            {
                
            }



        }
    }
}

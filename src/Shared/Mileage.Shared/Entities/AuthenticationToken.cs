using System;

namespace Mileage.Shared.Entities
{
    public class AuthenticationToken : AggregateRoot
    {
        public static string CreateId(string token)
        {
            return "AuthenticationTokens/" + token;
        }

        public DateTimeOffset CreatedDate { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
        public Client Client { get; set; }
    }
}
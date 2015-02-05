namespace Mileage.Shared.Entities
{
    public class AuthenticationToken : AggregateRoot
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
using Mileage.Shared.Entities;

namespace Mileage.Client.Windows.WebServices
{
    public class Session
    {
        public AuthenticationToken Token { get; set; }
        public User CurrentUser { get; set; }
    }
}
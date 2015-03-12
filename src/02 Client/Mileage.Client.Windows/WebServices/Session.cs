using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.WebServices
{
    public class Session
    {
        public AuthenticationToken Token { get; set; }
        public User CurrentUser { get; set; }

        /// <summary>
        /// Clears the session.
        /// </summary>
        public void Clear()
        {
            this.Token = null;
            this.CurrentUser = null;
        }
    }
}
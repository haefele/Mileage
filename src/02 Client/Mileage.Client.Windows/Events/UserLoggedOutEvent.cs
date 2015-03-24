using LiteGuard;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.Events
{
    public class UserLoggedOutEvent
    {
        public User User { get; private set; }

        public UserLoggedOutEvent(User user)
        {
            Guard.AgainstNullArgument("user", user);

            this.User = user;
        }
    }
}
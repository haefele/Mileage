using LiteGuard;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.Events
{
    public class UserLoggingOutEvent
    {
        public User User { get; private set; }

        public UserLoggingOutEvent(User user)
        {
            Guard.AgainstNullArgument("user", user);

            this.User = user;
        }
    }
}
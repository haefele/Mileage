using LiteGuard;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Windows.Events
{
    public class UserLoggedInEvent
    {
        public User User { get; private set; }

        public UserLoggedInEvent(User user)
        {
            Guard.AgainstNullArgument("user", user);

            this.User = user;
        }
    }
}
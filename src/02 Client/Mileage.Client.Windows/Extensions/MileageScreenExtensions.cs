using Caliburn.Micro;

namespace Mileage.Client.Windows.Extensions
{
    public static class MileageScreenExtensions
    {
        public static void TryActivate(this object potentialActivatable)
        {
            ScreenExtensions.TryActivate(potentialActivatable);
        }

        public static void TryDeactivate(this object potentialDeactivatable, bool close)
        {
            ScreenExtensions.TryDeactivate(potentialDeactivatable, close);
        }
    }
}
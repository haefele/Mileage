namespace Mileage.Shared.Entities.Mileage
{
    public class MileageInternalSettings : AggregateRoot
    {
        public static string CreateId()
        {
            return "MileageInternalSettings/Global";
        }

        public bool IsAdminUserCreated { get; set; }
    }
}
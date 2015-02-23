namespace Mileage.Shared.Entities.Mileage
{
    public class MileageSettings : AggregateRoot
    {
        public static string CreateId()
        {
            return "MileageSettings/Global";
        }

        public bool IsAdminUserCreated { get; set; }
    }
}
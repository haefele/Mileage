namespace Mileage.Shared.Entities.Mileage
{
    public class MileageSettings : AggregateRoot
    {
        public static string CreateId()
        {
            return "MileageSettings/Global";
        }

        public string DefaultEmailSuffix { get; set; }
    }
}
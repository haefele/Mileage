namespace Mileage.Shared.Entities.Mileage
{
    public class MileageSettings : AggregateRoot
    {
        public static string CreateId()
        {
            return "MileageSettings/Global";
        }

        public MileageSettings()
        {
            this.DefaultEmailSuffix = string.Empty;
        }

        public string DefaultEmailSuffix { get; set; }
    }
}
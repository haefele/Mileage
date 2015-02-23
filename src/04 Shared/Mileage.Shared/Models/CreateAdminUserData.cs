namespace Mileage.Shared.Models
{
    public class CreateAdminUserData
    {
        public string Username { get; set; }
        public byte[] PasswordMD5Hash { get; set; }
        public string EmailAddress { get; set; }
        public string Language { get; set; }
    }
}
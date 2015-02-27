namespace Mileage.Shared.Models
{
    public class LoginData
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordMD5Hash { get; set; }
    }
}
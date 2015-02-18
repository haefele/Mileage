namespace Mileage.Shared.Models
{
    public class LoginData
    {
        public string Username { get; set; }
        public byte[] PasswordMD5Hash { get; set; }
    }
}
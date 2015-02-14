namespace Mileage.Shared.Models
{
    public class Login
    {
        public string Username { get; set; }
        public byte[] PasswordMD5Hash { get; set; }
    }
}
namespace AuthenticationMicroservice.Models.Request
{
    public class UserModel
    {
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? PasswordRepeated { get; set; }
    }
}

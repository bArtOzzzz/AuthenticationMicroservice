namespace AuthenticationMicroservice.Models.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}

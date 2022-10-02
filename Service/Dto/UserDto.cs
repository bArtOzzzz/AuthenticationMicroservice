namespace Services.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public RoleDto Role { get; set; }
    }
}

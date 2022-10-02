using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class UserModel
    {
        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }
}

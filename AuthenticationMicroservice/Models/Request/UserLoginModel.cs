using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }
}

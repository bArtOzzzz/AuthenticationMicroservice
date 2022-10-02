using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class UserPasswordModel
    {
        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }
}

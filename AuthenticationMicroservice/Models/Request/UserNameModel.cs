using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class UserNameModel
    {
        [Required]
        [StringLength(30)]
        public string Username { get; set; }
    }
}

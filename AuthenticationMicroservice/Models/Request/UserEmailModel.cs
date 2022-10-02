using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class UserEmailModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}

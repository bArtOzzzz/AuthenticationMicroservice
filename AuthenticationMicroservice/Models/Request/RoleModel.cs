using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Models.Request
{
    public class RoleModel
    {
        [Required]
        [StringLength(20)]
        public string Role { get; set; }
    }
}

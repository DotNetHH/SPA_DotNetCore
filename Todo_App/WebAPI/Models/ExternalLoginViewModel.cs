using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Name")]
        public string FullName { get; set; }
    }
}

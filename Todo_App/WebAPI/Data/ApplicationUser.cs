using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [MaxLength(50)]
        public string FullName { get; set; }
    }

    public class ApplicationRole : IdentityRole<int>
    {
    }
}

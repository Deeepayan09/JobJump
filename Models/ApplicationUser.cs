using Microsoft.AspNetCore.Identity;
namespace JobJump.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName{get;set; }
        public String? RoleName{get;set; }

        public string? ResumePath { get; set; }
    }
}
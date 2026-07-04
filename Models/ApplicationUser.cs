using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kintai.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "氏名")]
        public string? FullName { get; set; }
    }
}

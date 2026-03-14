using Microsoft.AspNetCore.Identity;

namespace Kintai.Models
{
    public class ApplicationUser : IdentityUser
    {
        // 必要なら追加プロパティ
        public string? FullName { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace e_ticaret1.Models
{
    public class User : IdentityUser // IdentityUser'dan türetin
    {
        // Kullanıcı adını ve şifreyi burada tanımlamaya gerek yok çünkü IdentityUser'da mevcut.
        public int Role { get; set; } // 1: Admin, 0: User
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
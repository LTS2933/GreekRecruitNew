using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekRecruit.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public String username { get; set; }
        public String password { get; set; }

        [NotMapped]
        public String confirmPassword { get; set; }
        public String? email { get; set; }

        public String role { get; set; } = "User";
    }
}

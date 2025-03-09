using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GreekRecruit.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String email { get; set; }
    }
}

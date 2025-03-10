using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Models
{
    public class SqlDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public SqlDataContext(DbContextOptions<SqlDataContext> options) 
            : base(options)
        {}

        
    }
}

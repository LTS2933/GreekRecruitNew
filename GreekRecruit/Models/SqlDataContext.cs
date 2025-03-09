using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Models
{
    public class SqlDataContext : DbContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options) 
            : base(options)
        {}

        public DbSet<User> Users { get; set; }
    }
}

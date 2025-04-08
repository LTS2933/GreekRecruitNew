using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Models
{
    public class SqlDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PNM> PNMs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventAttendance> EventsAttendance { get; set; }

        public DbSet<PNMVoteSession> PNMVoteSessions { get; set; }
        public SqlDataContext(DbContextOptions<SqlDataContext> options) 
            : base(options)
        {}

        
    }
}

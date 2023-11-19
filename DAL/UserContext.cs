using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class UserContext : DbContext
    {
        public UserContext()
        {

        }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
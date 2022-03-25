using identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace identity._Context
{
    public class MyContext:IdentityDbContext
    {
        public DbSet<Enployees>  Enploye { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
        
    }
}

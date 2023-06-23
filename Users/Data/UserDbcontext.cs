using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Models;

namespace Users.Data
{
    public class UserDbcontext : IdentityDbContext
    {
        public UserDbcontext(DbContextOptions<UserDbcontext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN"},
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
                ); ;
        }

        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; }
    
    }


}

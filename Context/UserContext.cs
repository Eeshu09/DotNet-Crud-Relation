using Crud.Model;
using Microsoft.EntityFrameworkCore;

namespace Crud.Context
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext>options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.users)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId);
        }
        public DbSet<User> users { get; set; }
        public DbSet<Product>Products { get; set; } 

    }
}

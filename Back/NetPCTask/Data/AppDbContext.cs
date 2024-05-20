using Microsoft.EntityFrameworkCore;
using NetPCTask.Models;

namespace NetPCTask.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("ContactDb"));
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(cat => cat.Contacts)
                .WithOne(con => con.Category)
                .HasForeignKey(con => con.CategoryId);

            modelBuilder.Entity<Subcategory>()
                .HasMany(scat => scat.Contacts)
                .WithOne(con => con.Subcategory)
                .HasForeignKey(con => con.SubcategoryId);

            modelBuilder.Entity<Subcategory>()
                .HasOne(scat => scat.Category)
                .WithMany(cat => cat.Subcategories)
                .HasForeignKey(scat => scat.CategoryId);

            modelBuilder.Entity<User>(entity => { entity.HasIndex(u => u.Email).IsUnique(); });
            modelBuilder.Entity<Contact>(entity => { entity.HasIndex(c => c.Email).IsUnique(); });
        }


    }
}

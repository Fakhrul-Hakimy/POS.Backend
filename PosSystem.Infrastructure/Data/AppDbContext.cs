using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PosSystem.Application.Interfaces;
using PosSystem.Domain.Entities;
using CategoryEntity = PosSystem.Domain.Entities.Category;
using ProductEntity = PosSystem.Domain.Entities.Product;
using UserEntity = PosSystem.Domain.Entities.User;


namespace PosSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints if needed
            modelBuilder.Entity<ProductEntity>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
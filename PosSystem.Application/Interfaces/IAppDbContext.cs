using Microsoft.EntityFrameworkCore;
using PosSystem.Domain.Entities;
using CategoryEntity = PosSystem.Domain.Entities.Category;
using ProductEntity = PosSystem.Domain.Entities.Product;
using UserEntity = PosSystem.Domain.Entities.User;

namespace PosSystem.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<CategoryEntity> Categories { get; set; }
        DbSet<ProductEntity> Products { get; set; }
        DbSet<UserEntity> Users { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
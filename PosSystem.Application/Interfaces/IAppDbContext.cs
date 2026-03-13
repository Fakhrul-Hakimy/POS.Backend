using Microsoft.EntityFrameworkCore;
using PosSystem.Domain.Entities;

namespace PosSystem.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<User> Users { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
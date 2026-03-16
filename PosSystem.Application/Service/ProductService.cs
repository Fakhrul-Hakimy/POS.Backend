using Microsoft.EntityFrameworkCore;
using PosSystem.Application.Interfaces;
using PosSystem.Domain.Entities;


namespace PosSystem.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IAppDbContext _context;

        public ProductService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool?> AddNewProduct (newProductDto dto, string createdBy)
        {
            try
            {
                
           
            var product = new Product
            {
                Name=dto.Name,
                Description=dto.Description,
                Price=dto.Price,
                CategoryId=dto.CategoryId,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow+TimeSpan.FromHours(8)
            };

            await _context.Products.AddAsync(product);
            var rows= await _context.SaveChangesAsync();

            return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<object?> GetAllProduct()
        {
            var products = await _context.Products
                    .AsNoTracking()
                    .Select(p => new ToListProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Description= p.Description,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category != null ? p.Category.Name : null
                    })
                    .ToListAsync();


            return await Task.FromResult<object?>(products);
        }
        
    }


    public class ToListProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName {get ; set;}


    }
}
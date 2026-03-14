using PosSystem.Application.DTOs;
using PosSystem.Application.Interfaces;
using PosSystem.Domain.Entities;
namespace PosSystem.Application.Service
{
    public class CategoryService : ICategoryService
    {


        private readonly IAppDbContext _context;

        public CategoryService(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<object?> GetAll()
        {
            var allCategory= _context.Categories.ToList();

            return await Task.FromResult<object?>(allCategory);
        }

        public async Task<bool> AddCategory(newCategoryDto DTO, string createdBy)
        {

            var category= new Category{
                Name = DTO.Name,
                Description = DTO.Description,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow+TimeSpan.FromHours(8) // Adjust for UTC+8 timezone
            };

           await _context.Categories.AddAsync(category);
           var rows= await _context.SaveChangesAsync();

           return rows >0;



        }

        public async Task<bool> UpdateCategory(int id, updateCategoryDto DTO, string updatedBy){
            var category = await _context.Categories.FindAsync(id);
            if(category == null){
                return false;
            }else{
                category.Name = DTO.Name;
                category.Description = DTO.Description;
                category.UpdatedBy = updatedBy;
                category.UpdatedAt = DateTime.UtcNow+TimeSpan.FromHours(8); // Adjust for UTC+8 timezone

                _context.Categories.Update(category);
                var rows= await _context.SaveChangesAsync();

                return rows >0;
            }
        }

        public async Task<bool> DeleteCategory(int id, string deletedBy){
            var category = await _context.Categories.FindAsync(id);
            if(category == null){
                return false;
            }else{
                _context.Categories.Remove(category);
                var rows= await _context.SaveChangesAsync();

                return rows >0;
            }
        }

        public async Task<object?> GetById(int id){
            var category = await _context.Categories.FindAsync(id);
            if(category == null){
                return null;
            }else{
                return category;
            }
        }
    }
}
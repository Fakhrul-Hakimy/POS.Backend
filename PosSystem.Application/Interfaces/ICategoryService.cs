using PosSystem.Application.DTOs;
namespace PosSystem.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<object?> GetAll();

        Task<bool> AddCategory(newCategoryDto DTO, string createdBy);

        Task<bool> UpdateCategory(int id, updateCategoryDto DTO, string updatedBy);

        Task<bool> DeleteCategory(int id, string deletedBy);

        Task<object?> GetById(int id);
    }
}
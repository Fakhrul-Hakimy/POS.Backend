namespace PosSystem.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool?> AddNewProduct (newProductDto dto, string createdBy);

        Task<object?> GetAllProduct();

        Task<bool?> EditProduct(updateProductDto dto, int id, string UpdateBy);

        Task<bool?> DeleteProduct(int id);

        Task<object?> GetProductById(int id);
        
    }
    
}
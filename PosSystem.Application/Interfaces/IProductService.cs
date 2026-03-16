namespace PosSystem.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool?> AddNewProduct (newProductDto dto, string createdBy);

        Task<object?> GetAllProduct();
        
    }
    
}
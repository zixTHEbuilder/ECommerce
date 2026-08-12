using ECommerce.Dtos;
using ECommerce.Helpers;

namespace ECommerce.Services
{
    public interface IStoreService
    {
        Task<PagedResult<ProductDto>?> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ProductDto?> GetByIdAsync(int userId, int productId);
        Task<PagedResult<ProductDto>?> GetByCategoryAsync(int userId, string category, int pageNumber, int pageSize);
        Task<PurchaseResult<BillDto>> PurchaseProductsAsync(int userId);
    }
}

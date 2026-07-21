using ECommerce.Dtos;

namespace ECommerce.Services
{
    public interface IStoreService
    {
        Task<PagedResult<ProductDto>> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}

using ECommerce.Data;
using ECommerce.Dtos;
using System.Collections;

namespace ECommerce.Services
{
    public class StoreService : IStoreService
    {
        private readonly StoreContext _store;
        public StoreService(StoreContext storeContext, IServiceProvider serviceProvider)
        {
            _store = storeContext;
        }
        public async Task<PagedResult<ProductDto>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}

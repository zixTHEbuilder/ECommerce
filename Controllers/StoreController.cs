using ECommerce.Dtos;
using ECommerce.Helpers;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(IStoreService store): ControllerBase
    {
        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts(int PageNumber = 1, int PageSize = 5)
        {
            if (PageNumber < 1 || PageSize < 1 || PageSize > 30)
                return BadRequest("Page Number and Page Size must be more than 1 and Page Size must be or less than 30");
            var prodcuts = store.GetAllProductsAsync(PageNumber, PageSize);

            if (prodcuts is null) return NotFound("No Products Found");

            return Ok(prodcuts);
        }
    }
}

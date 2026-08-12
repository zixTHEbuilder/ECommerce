using ECommerce.Dtos;
using ECommerce.Helpers;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController(IStoreService store): ControllerBase
    {
        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int PageNumber = 1,[FromQuery] int PageSize = 5)
        {
            if (PageNumber < 1 || PageSize < 1 || PageSize > 30)
                return BadRequest("Page Number and Page Size must be more than 1 and Page Size must be or less than 30");
            var prodcuts = store.GetAllProductsAsync(PageNumber, PageSize);

            if (prodcuts is null) return NotFound("No Products Found");

            return Ok(prodcuts);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            //ValidateAsync
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var product = await store.GetByIdAsync(userId, id);

            if (product is null) return NotFound($"Unable to find a product with ID {id}");

            return Ok(product);
        }
        [HttpGet("{category}")]
        public async Task<IActionResult> GetByCategory(string category,[FromQuery]  int pageNumber,[FromQuery] int pageSize)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var categoryProducts = await store.GetByCategoryAsync(userId,category, pageNumber, pageSize);

            if (categoryProducts is null) return NotFound($"No products under the Category : {category}");

            return Ok(categoryProducts);
        }
        [HttpPost]
        public async Task<IActionResult> PurchaseProduct()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var purchased = await store.PurchaseProductsAsync(userId);

            
            return Ok(purchased);
        }
    }
}

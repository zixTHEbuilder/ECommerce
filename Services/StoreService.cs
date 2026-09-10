using ECommerce.Data;
using ECommerce.Dtos;
using ECommerce.Helpers;
using ECommerce.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ECommerce.Services
{
    public class StoreService : IStoreService
    {
        private readonly StoreContext _store;
        private readonly AuthContext _auth;
        private readonly IServiceProvider _service;
        public StoreService(StoreContext storeContext, IServiceProvider serviceProvider, AuthContext auth)
        {
            _store = storeContext;
            _service = serviceProvider;
            _auth = auth;
        }
        public async Task<PagedResult<ProductDto>?> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _store.Products.CountAsync();

            var products = await _store.Products.OrderBy(o => o.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto(p))
                .ToListAsync();

            if (!products.Any()) return null;

            return new PagedResult<ProductDto>()
            {
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = products,
                TotalProducts = totalCount
            };
        }
        public async Task<ProductDto?> GetByIdAsync(int userId, int productId)
        {
            var product = await _store.Products.FindAsync(productId);
            if (product is null) return null;

            //implement feature to show whether the user already has the item 
            //var hasItem = await _store.UserProducts.AnyAsync(u => u.UserId == userId && u.ProductId == productId);

            return new ProductDto(product);
        }
        public async Task<PagedResult<ProductDto>?> GetByCategoryAsync(int userId, string category, int pageNumber, int pageSize)
        {
            var categoryProducts = await _store.Products
                .Where(c => c.Category == category)
                .Select(p => new ProductDto(p))
                .ToListAsync();

            var totalCount = categoryProducts.Count();
            if (totalCount is 0) return null;

            return new PagedResult<ProductDto>
            {
                // add page number and page size in controller and iservice 
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = categoryProducts,
                TotalProducts = totalCount
            };
        }

        public async Task<PurchaseResult<BillDto>> PurchaseProductsAsync(int userId)
        {
            var user = await _auth.User.FindAsync(userId);
            var cartItems = await _store.Cart.Where(c => c.userId == userId).ToListAsync();

            if (user is null || cartItems is null || !cartItems.Any())
                return new PurchaseResult<BillDto>(PurchaseError.NotFound, "No User or Cart Items found");

            var productIds = cartItems.Select(x => x.ProductId).ToList();
            var products = await _store.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p);

            var ownedProducts = await _store.UserProducts
                .Where(u => u.UserId == userId && productIds.Contains(u.ProductId))
                .ToDictionaryAsync(u => u.ProductId, u => u);

            var totalCost = cartItems.Sum(item => products[item.ProductId].Price * item.ProductQuantity);

            if (user.Balance < totalCost)
                return new PurchaseResult<BillDto>(PurchaseError.InsufficientBalance, "Insufficient Balance");

            using var authTransaction = await _auth.Database.BeginTransactionAsync();
            using var storeTransaction = await _store.Database.BeginTransactionAsync();

            try
            {
                user.Balance -= totalCost;
                var billItems = new List<BillItemDto>();

                foreach (var item in cartItems)
                {
                    var product = products[item.ProductId];
                    decimal subtotal = product.Price * item.ProductQuantity;

                    if (ownedProducts.TryGetValue(item.ProductId, out var alreadyOwned))
                    {
                        alreadyOwned.ProductQuantity += item.ProductQuantity;
                        alreadyOwned.LastPurchaseDate = DateTime.UtcNow;
                        _store.UserProducts.Update(alreadyOwned);
                    }
                    else
                    {
                        var newUserProduct = new UserProducts()
                        {
                            UserId = userId,
                            ProductId = item.ProductId,
                            PurchasePrice = product.Price,
                            ProductQuantity = item.ProductQuantity,
                            PurchaseDate = DateTime.UtcNow,
                            LastPurchaseDate = DateTime.UtcNow
                        };
                        await _store.UserProducts.AddAsync(newUserProduct);
                    }

                    billItems.Add(new BillItemDto(product.ProductName, product.Price, item.ProductQuantity, subtotal));
                }

                _store.Cart.RemoveRange(cartItems);

                await _auth.SaveChangesAsync();
                await _store.SaveChangesAsync();

                await authTransaction.CommitAsync();
                await storeTransaction.CommitAsync();

                var billDto = new BillDto(user.Username, billItems, totalCost, DateTime.UtcNow);
                return new PurchaseResult<BillDto>(PurchaseError.None, "Successfully Purchased", billDto);
            }
            catch (Exception e)
            {
                await authTransaction.RollbackAsync();
                await storeTransaction.RollbackAsync();
                Console.WriteLine($"Database Error : {e.Message}");
                return new PurchaseResult<BillDto>(PurchaseError.DatabaseError, "An unexpected error occurred while processing your purchase.");
            }
        }
        //implement a service to buy a single product right away without using cart if the user instantly wants to buy without adding to cart
    }
}
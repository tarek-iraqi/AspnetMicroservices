using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _distributedCache;

    public BasketRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<ShoppingCart> GetBasketAsync(string username)
    {
        var cart = await _distributedCache.GetStringAsync(username);

        if (string.IsNullOrWhiteSpace(cart))
            return default;

        return JsonSerializer.Deserialize<ShoppingCart>(cart);
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart cart)
    {
        await _distributedCache.SetStringAsync(cart.Username, JsonSerializer.Serialize(cart));

        return await GetBasketAsync(cart.Username);
    }

    public async Task DeleteBasketAsync(string username)
        => await _distributedCache.RemoveAsync(username);
}

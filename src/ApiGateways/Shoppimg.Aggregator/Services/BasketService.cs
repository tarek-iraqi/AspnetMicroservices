using Shoppimg.Aggregator.Extensions;
using Shoppimg.Aggregator.Models;

namespace Shoppimg.Aggregator.Services;

public class BasketService
{
    private readonly HttpClient _httpClient;

    public BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BasketModel> GetBasket(string username)
    {
        var result = await _httpClient.GetAsync($"/api/v1/Basket/{username}");
        return await result.ReadContentAs<BasketModel>();
    }
}

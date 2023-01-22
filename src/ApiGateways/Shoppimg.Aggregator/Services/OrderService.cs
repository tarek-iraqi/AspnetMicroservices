using Shoppimg.Aggregator.Extensions;
using Shoppimg.Aggregator.Models;

namespace Shoppimg.Aggregator.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
    {
        var result = await _httpClient.GetAsync($"/api/v1/Order/{username}");
        return await result.ReadContentAs<List<OrderResponseModel>>();
    }
}

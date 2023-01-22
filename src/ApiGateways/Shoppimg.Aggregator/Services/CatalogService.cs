using Shoppimg.Aggregator.Extensions;
using Shoppimg.Aggregator.Models;

namespace Shoppimg.Aggregator.Services;

public class CatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        var result = await _httpClient.GetAsync("/api/v1/Catalog");
        return await result.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        var result = await _httpClient.GetAsync($"/api/v1/Catalog/{id}");
        return await result.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        var result = await _httpClient.GetAsync($"/api/v1/Catalog/GetProductsByCategory/{category}");
        return await result.ReadContentAs<List<CatalogModel>>();
    }
}

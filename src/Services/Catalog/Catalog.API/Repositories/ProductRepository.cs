using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }
    public async Task<IEnumerable<Product>> GetProductsAsync()
        => await _catalogContext.Products.Find(x => true).ToListAsync();

    public async Task<Product> GetProductByIdAsync(string id)
        => await _catalogContext.Products.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    public async Task CreateProductAsync(Product product)
        => await _catalogContext.Products.InsertOneAsync(product);

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updateResult = await _catalogContext.Products.ReplaceOneAsync(x => x.Id == product.Id, product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var deleteResult = await _catalogContext.Products.DeleteOneAsync(x => x.Id == id);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
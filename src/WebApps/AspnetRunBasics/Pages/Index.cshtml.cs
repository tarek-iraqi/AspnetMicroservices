using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetRunBasics.Pages;

public class IndexModel : PageModel
{
    private readonly CatalogService _catalogService;
    private readonly BasketService _basketService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(CatalogService catalogService,
        BasketService basketService,
        ILogger<IndexModel> logger)
    {
        _catalogService = catalogService;
        _basketService = basketService;
        _logger = logger;
    }

    public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        ProductList = await _catalogService.GetCatalog();
        _logger.LogInformation("Getting products from catalog");
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        var product = await _catalogService.GetCatalog(productId);



        var userName = "swn";
        var basket = await _basketService.GetBasket(userName);

        basket.Items.Add(new BasketItemModel
        {
            ProductId = productId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = 1,
            Color = "Black"
        });

        var basketUpdated = await _basketService.UpdateBasket(basket);
        return RedirectToPage("Cart");
    }
}

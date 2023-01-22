using Microsoft.AspNetCore.Mvc;
using Shoppimg.Aggregator.Models;
using Shoppimg.Aggregator.Services;
using System.Net;

namespace Shoppimg.Aggregator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingController : ControllerBase
{
    private readonly CatalogService _catalogService;
    private readonly BasketService _basketService;
    private readonly OrderService _orderService;

    public ShoppingController(CatalogService catalogService,
        BasketService basketService,
        OrderService orderService)
    {
        _catalogService = catalogService;
        _basketService = basketService;
        _orderService = orderService;
    }

    [HttpGet("{userName}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
    {
        var basket = await _basketService.GetBasket(userName);

        foreach (var item in basket.Items)
        {
            var product = await _catalogService.GetCatalog(item.ProductId);

            // set additional product fields
            item.ProductName = product.Name;
            item.Category = product.Category;
            item.Summary = product.Summary;
            item.Description = product.Description;
            item.ImageFile = product.ImageFile;
        }

        var orders = await _orderService.GetOrdersByUsername(userName);

        var shoppingModel = new ShoppingModel
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }
}

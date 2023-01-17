using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountgrpcservice;

    public BasketController(IBasketRepository basketRepository,
        DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository;
        _discountgrpcservice = discountGrpcService;
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket([FromRoute] string username)
    {
        var basket = await _basketRepository.GetBasketAsync(username);

        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart cart)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await _discountgrpcservice.GetDiscount(item.ProductName);

            item.Price -= coupon?.Amount ?? 0;
        }

        return Ok(await _basketRepository.UpdateBasketAsync(cart));
    }


    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteBasket([FromRoute] string username)
    {
        await _basketRepository.DeleteBasketAsync(username);

        return Ok();
    }
}

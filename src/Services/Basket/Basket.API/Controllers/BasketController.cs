using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountgrpcservice;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IBasketRepository basketRepository,
        DiscountGrpcService discountGrpcService,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _basketRepository = basketRepository;
        _discountgrpcservice = discountGrpcService;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
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

    [HttpPost("Checkout")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);

        if (basket is null) return BadRequest();

        var basketCheckoutMsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        basketCheckoutMsg.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(basketCheckoutMsg);

        await _basketRepository.DeleteBasketAsync(basketCheckout.UserName);

        return Accepted();
    }
}

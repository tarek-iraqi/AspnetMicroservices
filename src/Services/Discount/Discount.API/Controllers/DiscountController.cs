using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    [HttpGet("{productName}", Name = "GetDiscount")]
    [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Coupon>> GetDiscount([FromRoute] string productName)
    {
        var coupon = await _discountRepository.GetDiscountAsync(productName);

        if (coupon is null) return NotFound();

        return Ok(coupon);
    }


    [HttpPost]
    public async Task<ActionResult<bool>> CreateDiscount([FromBody] Coupon coupon)
    {
        await _discountRepository.CreateDiscountAsync(coupon);

        return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
    }


    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
        => Ok(await _discountRepository.UpdateDiscountAsync(coupon));


    [HttpDelete("{productName}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteDiscount([FromRoute] string productName)
        => Ok(await _discountRepository.DeleteDiscountAsync(productName));
}

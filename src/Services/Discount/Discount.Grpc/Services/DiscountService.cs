using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountrepository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository,
        ILogger<DiscountService> logger,
        IMapper mapper)
    {
        _discountrepository = discountRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountrepository.GetDiscountAsync(request.ProductName);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product = {request.ProductName} not found"));

        _logger.LogInformation($"Discount is found for product = {request.ProductName}");

        var couponModel = _mapper.Map<CouponModel>(coupon);

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _discountrepository.CreateDiscountAsync(coupon);

        _logger.LogInformation($"Discount is created successfully for product = {coupon.ProductName}");

        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);

        await _discountrepository.UpdateDiscountAsync(coupon);

        _logger.LogInformation($"Discount is updates successfully for product = {coupon.ProductName}");

        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _discountrepository.DeleteDiscountAsync(request.ProductName);

        _logger.LogInformation($"Discount is deleted successfully for product = {request.ProductName}");

        return new DeleteDiscountResponse { IsDeleted = result };
    }
}

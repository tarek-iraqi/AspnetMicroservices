using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountprotoserviceclient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountprotoserviceclient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var getDiscountRequest = new GetDiscountRequest { ProductName = productName };

        return await _discountprotoserviceclient.GetDiscountAsync(getDiscountRequest);
    }
}

using Discount.Grpc;
using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, ProductDescription = "No Discount" };

        logger.LogInformation(
            $"Get discount for {coupon.ProductName}, Amount: {coupon.Amount}, ProductDescription: {coupon.ProductDescription}");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Internal request object"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Create discount for {coupon.ProductName}");
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Internal request object"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Update discount for {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        // Log para verificar la solicitud
        logger.LogInformation($"Received request to delete discount for product: {request.ProductName}");

        if (string.IsNullOrWhiteSpace(request.ProductName))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Product name must be provided"));
        }

        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
        {
            logger.LogWarning($"Coupon not found for product: {request.ProductName}");
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for {request.ProductName}"));
        }

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Successfully deleted discount for {request.ProductName}");

        return new DeleteDiscountResponse { Success = true };
    }
}
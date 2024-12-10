namespace Discount.gRPC.Models;

public class Coupon
{
    public int Id { get; set; }
    
    public string ProductName { get; set; } = default!;
    
    public string ProductDescription { get; set; } = default!;
    
    public decimal Amount { get; set; }
}
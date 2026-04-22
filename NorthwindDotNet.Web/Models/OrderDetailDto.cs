namespace NorthwindDotNet.Web.Models;

public class OrderDetailDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal? ShippingFee { get; set; }
    public float? TaxRate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    public EmployeeRefDto? Employee { get; set; }
    public CompanyRefDto? Customer { get; set; }
    public CompanyRefDto? Shipper { get; set; }
    public OrderStatusDto? OrderStatus { get; set; }
    public List<OrderLineItemDto> OrderDetails { get; set; } = [];
}

public class OrderLineItemDto
{
    public int OrderDetailId { get; set; }
    public short? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public float? Discount { get; set; }
    public ProductRefDto? Product { get; set; }
    public OrderDetailStatusDto? OrderDetailStatus { get; set; }

    public decimal LineTotal =>
        (UnitPrice ?? 0m) * (Quantity ?? 0) * (1m - (decimal)(Discount ?? 0f));
}

public class ProductRefDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
}

public class OrderDetailStatusDto
{
    public int OrderDetailStatusId { get; set; }
    public string? OrderDetailStatusName { get; set; }
}

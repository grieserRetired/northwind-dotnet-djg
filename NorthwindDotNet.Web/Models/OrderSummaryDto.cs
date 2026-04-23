namespace NorthwindDotNet.Web.Models;

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public decimal? ShippingFee { get; set; }
    public EmployeeRefDto? Employee { get; set; }
    public CompanyRefDto? Customer { get; set; }
    public CompanyRefDto? Shipper { get; set; }
    public OrderStatusDto? OrderStatus { get; set; }
}

public class EmployeeRefDto
{
    public int EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class CompanyRefDto
{
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
}

public class OrderStatusDto
{
    public int OrderStatusId { get; set; }
    public string? OrderStatusName { get; set; }
}

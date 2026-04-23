namespace NorthwindDotNet.Web.Models;

public class SalesByEmployeeDto
{
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class SalesByProductDto
{
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }
    public int UnitsSold { get; set; }
    public decimal TotalRevenue { get; set; }
}

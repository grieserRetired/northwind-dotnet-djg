namespace NorthwindDotNet.Web.Models;

public class ProductDto
{
    public int ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? StandardUnitCost { get; set; }
    public short? ReorderLevel { get; set; }
    public short? TargetLevel { get; set; }
    public string? QuantityPerUnit { get; set; }
    public bool? Discontinued { get; set; }
    public ProductCategoryDto? ProductCategory { get; set; }
}

public class ProductCategoryDto
{
    public int ProductCategoryId { get; set; }
    public string? ProductCategoryName { get; set; }
}

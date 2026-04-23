namespace NorthwindDotNet.Web.Models;

public class ProductDetailDto
{
    public int ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? StandardUnitCost { get; set; }
    public short? ReorderLevel { get; set; }
    public short? TargetLevel { get; set; }
    public short? MinimumReorderQuantity { get; set; }
    public string? QuantityPerUnit { get; set; }
    public bool? Discontinued { get; set; }
    public ProductCategoryDto? ProductCategory { get; set; }
    public List<ProductVendorDto> ProductVendors { get; set; } = [];
}

public class ProductVendorDto
{
    public int ProductVendorId { get; set; }
    public CompanyDto? Vendor { get; set; }
}

public class CompanyDetailDto
{
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? BusinessPhone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? StateAbbrev { get; set; }
    public string? Zip { get; set; }
    public string? Website { get; set; }
    public string? Notes { get; set; }
    public CompanyTypeDto? CompanyType { get; set; }
    public List<ContactDto> Contacts { get; set; } = [];
}

public class ContactDto
{
    public int ContactId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? JobTitle { get; set; }
    public string? EmailAddress { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
}

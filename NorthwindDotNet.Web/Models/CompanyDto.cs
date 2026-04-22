namespace NorthwindDotNet.Web.Models;

public class CompanyDto
{
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? BusinessPhone { get; set; }
    public string? City { get; set; }
    public string? StateAbbrev { get; set; }
    public CompanyTypeDto? CompanyType { get; set; }
}

public class CompanyTypeDto
{
    public int CompanyTypeId { get; set; }
    public string? CompanyTypeName { get; set; }
}

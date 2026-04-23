using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Reports;

public class SalesByProductModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public SalesByProductModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public List<SalesByProductDto> Results { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(DateTime? from = null, DateTime? to = null)
    {
        From = from;
        To   = to;

        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            var url = "api/reports/sales-by-product";
            var qs  = new List<string>();
            if (from.HasValue) qs.Add($"from={from.Value:yyyy-MM-dd}");
            if (to.HasValue)   qs.Add($"to={to.Value:yyyy-MM-dd}");
            if (qs.Any()) url += "?" + string.Join("&", qs);

            Results = await client.GetFromJsonAsync<List<SalesByProductDto>>(url) ?? [];
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load report: {ex.Message}";
        }
    }
}

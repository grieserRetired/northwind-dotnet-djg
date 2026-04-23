using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Reports;

public class SalesByEmployeeModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public SalesByEmployeeModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public List<SalesByEmployeeDto> Results { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(DateTime? from = null, DateTime? to = null)
    {
        From = from;
        To   = to;

        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            var url = "api/reports/sales-by-employee";
            var qs  = new List<string>();
            if (from.HasValue) qs.Add($"from={from.Value:yyyy-MM-dd}");
            if (to.HasValue)   qs.Add($"to={to.Value:yyyy-MM-dd}");
            if (qs.Any()) url += "?" + string.Join("&", qs);

            Results = await client.GetFromJsonAsync<List<SalesByEmployeeDto>>(url) ?? [];
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load report: {ex.Message}";
        }
    }
}

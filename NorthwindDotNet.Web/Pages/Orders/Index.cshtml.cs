using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Orders;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public List<OrderSummaryDto> Orders { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Orders = await client.GetFromJsonAsync<List<OrderSummaryDto>>("api/orders") ?? [];
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load orders: {ex.Message}";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Reports;

public class InvoiceModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public InvoiceModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public int? OrderId { get; set; }
    public OrderDetailDto? Order { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(int? orderId = null)
    {
        OrderId = orderId;
        if (orderId is null) return;

        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Order = await client.GetFromJsonAsync<OrderDetailDto>($"api/orders/{orderId}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Order not found — leave Order null, UI handles it
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load order: {ex.Message}";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Orders;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DetailsModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public OrderDetailDto? Order { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Order = await client.GetFromJsonAsync<OrderDetailDto>($"api/orders/{id}");
            if (Order is null)
                return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load order: {ex.Message}";
        }
        return Page();
    }
}

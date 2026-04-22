using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public List<ProductDto> Products { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Products = await client.GetFromJsonAsync<List<ProductDto>>("api/products") ?? [];
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load products: {ex.Message}";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public DetailsModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public ProductDetailDto? Product { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Product = await client.GetFromJsonAsync<ProductDetailDto>($"api/products/{id}");
            if (Product is null)
                return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load product: {ex.Message}";
        }
        return Page();
    }
}

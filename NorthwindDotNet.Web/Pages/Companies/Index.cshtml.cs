using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Companies;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public List<CompanyDto> Companies { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");
            Companies = await client.GetFromJsonAsync<List<CompanyDto>>("api/companies") ?? [];
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load companies: {ex.Message}";
        }
    }
}

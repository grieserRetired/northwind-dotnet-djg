using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindDotNet.Web.Models;

namespace NorthwindDotNet.Web.Pages.Companies;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public DetailsModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public CompanyDetailDto? Company { get; set; }
    public List<OrderSummaryDto> Orders { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("NorthwindApi");

            var companyTask = client.GetFromJsonAsync<CompanyDetailDto>($"api/companies/{id}");
            var ordersTask  = client.GetFromJsonAsync<List<OrderSummaryDto>>($"api/orders?companyId={id}");

            await Task.WhenAll(companyTask, ordersTask);

            Company = await companyTask;
            Orders  = await ordersTask ?? [];

            if (Company is null)
                return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load company: {ex.Message}";
        }
        return Page();
    }
}

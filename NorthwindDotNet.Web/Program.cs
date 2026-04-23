var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var apiBaseUrl = Environment.GetEnvironmentVariable("ApiBaseUrl") ?? "http://localhost:5132";
builder.Services.AddHttpClient("NorthwindApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

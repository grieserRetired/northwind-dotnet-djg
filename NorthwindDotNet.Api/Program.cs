using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NorthwindDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

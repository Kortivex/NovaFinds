using Microsoft.EntityFrameworkCore;
using NovaFinds.API.Handlers;
using NovaFinds.API.Handlers.Auth;
using NovaFinds.CORE.Contracts;
using NovaFinds.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbContext, ApplicationDbContext>();

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = ApiKeySchemeOptions.AuthenticateScheme;
        options.DefaultChallengeScheme = ApiKeySchemeOptions.ChallengeScheme;
    })
    .AddScheme<ApiKeySchemeOptions, ApiKeySchemeHandler>(
        ApiKeySchemeOptions.AuthenticateScheme,
        options => { options.HeaderName = "X-Api-Key"; });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/products", ProductsHandler.GetProducts)
    .WithName("GetProducts")
    .RequireAuthorization();

app.Run();
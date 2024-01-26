using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovaFinds.API.Handlers;
using NovaFinds.API.Handlers.Auth;
using NovaFinds.Application.Services;
using NovaFinds.CORE.Contracts;
using NovaFinds.CORE.Domain;
using NovaFinds.DAL.Context;
using NovaFinds.IFR.Logger;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Services.
builder.Services.AddScoped<IDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ICartRepository, CartService>();
builder.Services.AddScoped<IOrderRepository, OrderService>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductService>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleService>();

// Handlers.
builder.Services.AddScoped<ProductHandler>();
builder.Services.AddScoped<CategoryHandler>();
builder.Services.AddScoped<UserHandler>();

// Identity.
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Ignore References in Json Deserializer.
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

// Add DB Context.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication.
builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = ApiKeySchemeOptions.AuthenticateScheme;
        options.DefaultChallengeScheme = ApiKeySchemeOptions.ChallengeScheme;
        options.DefaultScheme = ApiKeySchemeOptions.AuthenticateScheme;
    })
    .AddScheme<ApiKeySchemeOptions, ApiKeySchemeHandler>(
        ApiKeySchemeOptions.AuthenticateScheme,
        options => { options.HeaderName = "X-Api-Key"; });

// Add Authorization.
builder.Services.AddAuthorization();

Logger.Debug("REST API services configured!");

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Handlers
app.MapPost("/users", (UserHandler handler, HttpRequest request) => handler.PostUser(request))
    .WithName("PostUser")
    .RequireAuthorization();

app.MapGet("/products", (ProductHandler handler, HttpRequest request) => handler.GetProducts(request))
    .WithName("GetProducts")
    .RequireAuthorization();

app.MapGet("/products/{id}", (ProductHandler handler, HttpRequest request, string id) => handler.GetProduct(request, id))
    .WithName("GetProduct")
    .RequireAuthorization();

app.MapGet("/categories", (CategoryHandler handler, HttpRequest request) => handler.GetCategories(request))
    .WithName("GetCategories")
    .RequireAuthorization();

app.MapGet("/categories/{id}", (CategoryHandler handler, HttpRequest request, string id) => handler.GetCategory(request, id))
    .WithName("GetCategory")
    .RequireAuthorization();

Logger.Debug("REST API app configured!");

app.Run();
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
builder.Services.AddScoped<ICartItemRepository, CartItemService>();
builder.Services.AddScoped<IOrderRepository, OrderService>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductService>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleService>();
builder.Services.AddTransient<IEmailSender, EmailService>();

// Handlers.
builder.Services.AddScoped<ProductHandler>();
builder.Services.AddScoped<CategoryHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<CartHandler>();
builder.Services.AddScoped<OrderHandler>();
builder.Services.AddScoped<EmailHandler>();

// Identity.
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Password Hasher configuration
builder.Services.Configure<PasswordHasherOptions>(options =>
        options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
    );

// Ignore References in Json Deserializer.
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

// Add DB Context.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    });

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
//  - USERS
app.MapPost("/users", (UserHandler handler, HttpRequest request) => handler.PostUser(request))
    .WithName("PostUser")
    .RequireAuthorization();

app.MapPut("/users/{username}", (UserHandler handler, HttpRequest request, string username) => handler.PutUser(request, username))
    .WithName("PutUser")
    .RequireAuthorization();

app.MapGet("/users", (UserHandler handler, HttpRequest request) => handler.GetUsers(request))
    .WithName("GetUsers")
    .RequireAuthorization();

app.MapDelete("/users/{username}", (UserHandler handler, HttpRequest request, string username) => handler.DeleteUser(request, username))
    .WithName("DeleteUser")
    .RequireAuthorization();

//  - USER - CARTS
app.MapGet("/users/{username}/carts", (UserHandler handler, HttpRequest request, string username) => handler.GetUsersCart(request, username))
    .WithName("GetUsersCart")
    .RequireAuthorization();

//  - CARTS
app.MapPost("/carts", (CartHandler handler, HttpRequest request) => handler.PostCarts(request))
    .WithName("PostCarts")
    .RequireAuthorization();

app.MapDelete("/carts/{cartId:int}", (CartHandler handler, HttpRequest request, int cartId) => handler.DeleteCart(request, cartId))
    .WithName("DeleteCart")
    .RequireAuthorization();

app.MapPost("/carts/{cartId:int}/items", (CartHandler handler, HttpRequest request, int cartId) => handler.PostCartItems(request, cartId))
    .WithName("PostCartItems")
    .RequireAuthorization();

app.MapPut("/carts/{cartId:int}/items/{itemId:int}", (CartHandler handler, HttpRequest request, int cartId, int itemId) => handler.PutCartItems(request, cartId, itemId))
    .WithName("PutCartItems")
    .RequireAuthorization();

app.MapGet("/carts/{cartId:int}/items", (CartHandler handler, HttpRequest request, int cartId) => handler.GetCartItems(request, cartId))
    .WithName("GetCartItems")
    .RequireAuthorization();

//  - CART - PRODUCTS
app.MapDelete("/carts/{cartId:int}/item-products/{productId:int}", (CartHandler handler, HttpRequest request, int cartId, int productId) => handler.DeleteCartItems(request, cartId, productId))
    .WithName("DeleteCartItems")
    .RequireAuthorization();

//  - PRODUCTS
app.MapGet("/products", (ProductHandler handler, HttpRequest request) => handler.GetProducts(request))
    .WithName("GetProducts")
    .RequireAuthorization();

app.MapGet("/products/{id}", (ProductHandler handler, HttpRequest request, string id) => handler.GetProduct(request, id))
    .WithName("GetProduct")
    .RequireAuthorization();

//  - CATEGORIES
app.MapGet("/categories", (CategoryHandler handler, HttpRequest request) => handler.GetCategories(request))
    .WithName("GetCategories")
    .RequireAuthorization();

app.MapGet("/categories/{id}", (CategoryHandler handler, HttpRequest request, string id) => handler.GetCategory(request, id))
    .WithName("GetCategory")
    .RequireAuthorization();

//  - ORDERS
//  USERS - ORDERS
app.MapGet("/users/{username}/orders", (UserHandler handler, HttpRequest request, string username) => handler.GetUsersOrders(request, username))
    .WithName("GetUsersOrders")
    .RequireAuthorization();

app.MapPost("/users/{username}/orders", (UserHandler handler, HttpRequest request, string username) => handler.PostUsersOrders(request, username))
    .WithName("PostUsersOrders")
    .RequireAuthorization();

app.MapPut("/users/{username}/orders/{id:int}", (UserHandler handler, HttpRequest request, string username, int id) => handler.PutUsersOrders(request, username, id))
    .WithName("PutUsersOrders")
    .RequireAuthorization();

//  ORDERS - PRODUCTS
app.MapGet("/orders/{id:int}/products", (OrderHandler handler, HttpRequest request, int id) => handler.GetOrdersProducts(request, id))
    .WithName("GetOrdersProducts")
    .RequireAuthorization();

app.MapPost("/orders/{id:int}/products", (OrderHandler handler, HttpRequest request, int id) => handler.PostOrdersProducts(request, id))
    .WithName("PostOrdersProducts")
    .RequireAuthorization();

//  EMAILS
app.MapPost("/emails", (EmailHandler handler, HttpRequest request) => handler.PostEmails(request))
    .WithName("PostEmails")
    .RequireAuthorization();

Logger.Debug("REST API app configured!");

app.Run();
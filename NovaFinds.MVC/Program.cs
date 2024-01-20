/**********************
 *
 *   SERVICES REGION
 *
 **********************/

#region SERVICES

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using NovaFinds.CORE.Domain;
using NovaFinds.DAL.Context;
using SmartBreadcrumbs.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Database connection string configuration
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Settings
builder.Services.AddIdentity<User, Role>(
        options => {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
            options.Lockout.MaxFailedAccessAttempts = 9999;

            // Password requirements
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        })
    .AddRoles<Role>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookies configuration
builder.Services.Configure<CookiePolicyOptions>(
    options => {
        options.CheckConsentNeeded = _ => true;
        options.MinimumSameSitePolicy = SameSiteMode.Strict;
    });

// Config Token Provider
builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
builder.Services.AddDistributedMemoryCache();

// Config Razor View Engine
builder.Services.Configure<RazorViewEngineOptions>(
    o => {
        o.ViewLocationFormats.Add($"/Views/Company/About{RazorViewEngine.ViewExtension}");
        o.ViewLocationFormats.Add($"/Views/Company/Terms{RazorViewEngine.ViewExtension}");
        o.ViewLocationFormats.Add($"/Views/Help/Refund{RazorViewEngine.ViewExtension}");
        o.ViewLocationFormats.Add($"/Views/Help/Shipping{RazorViewEngine.ViewExtension}");
    });

// Config Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options => builder.Configuration.Bind("CookieSettings", options));

// Config Session
builder.Services.AddSession(
    options => {
        // Set a timeout for session.
        options.IdleTimeout = TimeSpan.FromDays(7);
        options.Cookie.Name = "Session";
        options.Cookie.HttpOnly = true;

        // Make the session cookie essential
        options.Cookie.IsEssential = true;
    });

// Config Cookie Session 
builder.Services.ConfigureApplicationCookie(
    options => {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(5);
        options.SlidingExpiration = true;
    });

builder.Services.AddSingleton(builder.Configuration);

// Email provider config
// services.AddTransient<IEmailSender, EmailSender>();

// Adds Razor Pages services to the application's services container.
builder.Services.AddRazorPages();

// Add own Services
builder.Services.AddBreadcrumbs(
    typeof(Program).Assembly,
    options => {
        options.TagName = "div";
        options.TagClasses = "container";
        options.OlClasses = "breadcrumb breadcrumb-padding-left";
        options.LiClasses = "breadcrumb-item";
        options.ActiveLiClasses = "breadcrumb-item active";
    });

// Ignore References in Json Deserializer
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

// Add services to the container.
builder.Services.AddControllersWithViews();

#endregion

/**********************
 *
 *     APP REGION
 *
 **********************/

#region APP

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

#endregion
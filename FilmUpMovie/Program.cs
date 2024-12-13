using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("FilmUpMovieContext");
builder.Services.AddDbContext<FilmUpMovieContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity setup
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FilmUpMovieContext>();

builder.Services.AddRazorPages();

// Enable session services
builder.Services.AddDistributedMemoryCache();  // Add memory cache for sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;  // Ensure cookies are accessible only via HTTP(S)
    options.Cookie.IsEssential = true;  // Make sure the session cookie is essential
});

// Authorization policies
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

// Authorization handlers for all modules
builder.Services.AddScoped<IAuthorizationHandler, MovieIsOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MovieAdministratorsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MovieManagerAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ShowTimeIsOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShowTimeAdministratorsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShowTimeManagerAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, CinemaIsOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CinemaAdministratorsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CinemaManagerAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ShowRoomIsOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShowRoomAdministratorsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShowRoomManagerAuthorizationHandler>();

// HR authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, LeaveApplicationAdminAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, LeaveApplicationIsOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, LeaveApplicationManagerAuthorizationHandler>();

// Food, Beverage, and Combo authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, FoodOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, FoodAdminAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, FoodManagerAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, BeverageOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, BeverageAdminAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, BeverageManagerAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ComboOwnerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ComboAdminAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ComboManagerAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FilmUpMovieContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");
    if (string.IsNullOrEmpty(testUserPw))
    {
        testUserPw = "DefaultPassword123!"; // Set a default password if not provided
    }
    await DbInitializer.Initialize(services, testUserPw); // Initialize seed data
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
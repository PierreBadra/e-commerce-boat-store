using Microsoft.EntityFrameworkCore;
using pbadraH60A01.Models;
using Microsoft.AspNetCore.Identity;
using pbadraH60A01.Services;
using pbadraH60A01.DAL;
using pbadraH60Store;
using pbadraH60Store.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

if (!builder.Environment.IsDevelopment())
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
    var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
    builder.Services.AddDbContext<H60assignmentDbPbadraContext>(opt => opt.UseSqlServer(connectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException("Connection string 'H60assignmentDbPbadraContextConnection' not found."); 
    builder.Services.AddDbContext<H60assignmentDbPbadraContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<H60assignmentDbPbadraContext>();

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = "StoreAuth";
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();

var app = builder.Build();
await CreateUserRolesAndAdminUsers.Execute(app);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();
app.Run();
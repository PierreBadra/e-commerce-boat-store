using Microsoft.EntityFrameworkCore;
using pbadraH60Services.Services;
using pbadraH60Services.Models;
using pbadraH60Services.DAL;
using Microsoft.AspNetCore.Identity;
using pbadraH60Services.CalculateTaxes;
using pbadraH60Services.CheckCreditCard;
using pbadraH60Services.DTO;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()                     
              .AllowAnyMethod();                
    });
});

if (!builder.Environment.IsDevelopment())
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
    var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
    builder.Services.AddDbContext<H60assignment2DbPbadraContext>(opt => opt.UseSqlServer(connectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException("Connection string 'H60assignmentDbPbadraContextConnection' not found."); 
    builder.Services.AddDbContext<H60assignment2DbPbadraContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<H60assignment2DbPbadraContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository<CartItem>, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository<OrderDTO>, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository<OrderItemDTO>, OrderItemRepository>();
builder.Services.AddScoped<CheckCreditCardSoapClient>(serviceProvider =>
{
    var endpointConfiguration = CheckCreditCardSoapClient.EndpointConfiguration.CheckCreditCardSoap;
    return new CheckCreditCardSoapClient(endpointConfiguration);
});

builder.Services.AddScoped<CalculateTaxesSoapClient>(serviceProvider =>
{
    var endpointConfiguration = CalculateTaxesSoapClient.EndpointConfiguration.CalculateTaxesSoap;
    return new CalculateTaxesSoapClient(endpointConfiguration);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
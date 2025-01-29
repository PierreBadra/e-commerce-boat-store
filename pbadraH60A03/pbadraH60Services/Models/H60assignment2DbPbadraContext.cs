using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace pbadraH60Services.Models;

public partial class H60assignment2DbPbadraContext : IdentityDbContext<IdentityUser>
{
    public H60assignment2DbPbadraContext(DbContextOptions<H60assignment2DbPbadraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
    .HasOne(p => p.ProdCat)
    .WithMany(pc => pc.Products)
    .HasForeignKey(p => p.ProdCatId);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.ShoppingCart)
            .WithOne(sc => sc.Customer)
            .HasForeignKey<ShoppingCart>(sc => sc.CustomerId);

        modelBuilder.Entity<Customer>()
            .HasOne<IdentityUser>()
            .WithOne()
            .HasForeignKey<Customer>(c => c.UserId)
            .IsRequired();

        modelBuilder.Entity<ShoppingCart>()
            .HasMany(sc => sc.CartItems)
            .WithOne(ci => ci.ShoppingCart)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => pc.CategoryId);

        modelBuilder.Entity<Product>()
            .HasKey(p => p.ProductId);

        modelBuilder.Entity<Customer>()
            .HasKey(c => c.CustomerId);

        modelBuilder.Entity<ShoppingCart>()
            .HasKey(sc => sc.CartId);

        modelBuilder.Entity<CartItem>()
            .HasKey(ci => ci.CartItemId);

        modelBuilder.Entity<Order>()
            .HasKey(o => o.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.OrderItemId);

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { CategoryId = 1, ProdCat = "Sailboats" },
            new ProductCategory { CategoryId = 2, ProdCat = "Motorboats" },
            new ProductCategory { CategoryId = 3, ProdCat = "Fishing Boats" },
            new ProductCategory { CategoryId = 4, ProdCat = "Yachts" },
            new ProductCategory { CategoryId = 5, ProdCat = "Jet Skis" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, ProdCatId = 1, Description = "Beneteau Oceanis 30.1", Manufacturer = "Beneteau", Stock = 5, BuyPrice = 65000.00m, SellPrice = 75000.00m },
            new Product { ProductId = 2, ProdCatId = 1, Description = "Catalina 320", Manufacturer = "Catalina Yachts", Stock = 3, BuyPrice = 45000.00m, SellPrice = 55000.00m },
            new Product { ProductId = 3, ProdCatId = 1, Description = "Jeanneau Sun Odyssey 410", Manufacturer = "Jeanneau", Stock = 4, BuyPrice = 120000.00m, SellPrice = 140000.00m },
            new Product { ProductId = 4, ProdCatId = 1, Description = "Hunter 33", Manufacturer = "Hunter Marine", Stock = 2, BuyPrice = 35000.00m, SellPrice = 45000.00m },

            new Product { ProductId = 5, ProdCatId = 2, Description = "Sea Ray SPX 190", Manufacturer = "Sea Ray", Stock = 7, BuyPrice = 30000.00m, SellPrice = 35000.00m },
            new Product { ProductId = 6, ProdCatId = 2, Description = "Chaparral 23 SSi", Manufacturer = "Chaparral Boats", Stock = 5, BuyPrice = 55000.00m, SellPrice = 60000.00m },
            new Product { ProductId = 7, ProdCatId = 2, Description = "Bayliner VR5", Manufacturer = "Bayliner", Stock = 8, BuyPrice = 25000.00m, SellPrice = 30000.00m },
            new Product { ProductId = 8, ProdCatId = 2, Description = "Cobalt R30", Manufacturer = "Cobalt Boats", Stock = 3, BuyPrice = 75000.00m, SellPrice = 85000.00m },

            new Product { ProductId = 9, ProdCatId = 3, Description = "Lund 1775 Crossover XS", Manufacturer = "Lund Boats", Stock = 10, BuyPrice = 22000.00m, SellPrice = 26000.00m },
            new Product { ProductId = 10, ProdCatId = 3, Description = "Tracker Pro Guide V-175 Combo", Manufacturer = "Tracker Boats", Stock = 6, BuyPrice = 26000.00m, SellPrice = 30000.00m },
            new Product { ProductId = 11, ProdCatId = 3, Description = "Ranger Z518", Manufacturer = "Ranger Boats", Stock = 4, BuyPrice = 35000.00m, SellPrice = 40000.00m },
            new Product { ProductId = 12, ProdCatId = 3, Description = "Bass Cat Cougar FTD", Manufacturer = "Bass Cat Boats", Stock = 2, BuyPrice = 45000.00m, SellPrice = 50000.00m },

            new Product { ProductId = 13, ProdCatId = 4, Description = "Sunseeker Predator 55", Manufacturer = "Sunseeker", Stock = 1, BuyPrice = 800000.00m, SellPrice = 950000.00m },
            new Product { ProductId = 14, ProdCatId = 4, Description = "Princess V60", Manufacturer = "Princess Yachts", Stock = 1, BuyPrice = 950000.00m, SellPrice = 1150000.00m },
            new Product { ProductId = 15, ProdCatId = 4, Description = "Fairline Targa 45", Manufacturer = "Fairline", Stock = 2, BuyPrice = 600000.00m, SellPrice = 700000.00m },
            new Product { ProductId = 16, ProdCatId = 4, Description = "Azimut S6", Manufacturer = "Azimut", Stock = 1, BuyPrice = 1400000.00m, SellPrice = 1600000.00m },

            new Product { ProductId = 17, ProdCatId = 5, Description = "Sea-Doo GTX 170", Manufacturer = "Sea-Doo", Stock = 10, BuyPrice = 12000.00m, SellPrice = 15000.00m },
            new Product { ProductId = 18, ProdCatId = 5, Description = "Yamaha EX Deluxe", Manufacturer = "Yamaha", Stock = 8, BuyPrice = 8000.00m, SellPrice = 10000.00m },
            new Product { ProductId = 19, ProdCatId = 5, Description = "Kawasaki Jet Ski Ultra 310R", Manufacturer = "Kawasaki", Stock = 5, BuyPrice = 15000.00m, SellPrice = 18000.00m },
            new Product { ProductId = 20, ProdCatId = 5, Description = "Sea-Doo RXT-X 300", Manufacturer = "Sea-Doo", Stock = 7, BuyPrice = 14000.00m, SellPrice = 17000.00m }
        );
    }
}

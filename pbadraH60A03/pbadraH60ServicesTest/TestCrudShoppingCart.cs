using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DAL;
using pbadraH60Services.Models;

namespace pbadraH60ServicesTest;

public class TestCrudShoppingCart : IDisposable
{
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;
    private readonly H60assignment2DbPbadraContext _context;
    
    public TestCrudShoppingCart()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<H60assignment2DbPbadraContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _context = new H60assignment2DbPbadraContext(options);
        _shoppingCartRepository = new ShoppingCartRepository(_context);
        
        var customers = new List<Customer>
        {
            new Customer { Email = "customer@gmail.com", FirstName = "Cust", LastName = "omer", UserId = "userid1" },
            new Customer { Email = "customer2@gmail.com", FirstName = "Cust", LastName = "omer2", UserId = "userid2" },
            new Customer { Email = "customer3@gmail.com", FirstName = "Cust", LastName = "omer3", UserId = "userid3" },
            new Customer { Email = "customer4@gmail.com", FirstName = "Cust", LastName = "omer4", UserId = "userid4" }
        };
        _context.Customers.AddRange(customers);

        var shoppingCarts = new List<ShoppingCart>
        {
            new ShoppingCart { CustomerId = 1, DateCreated = DateTime.Today },
            new ShoppingCart { CustomerId = 2, DateCreated = DateTime.Today },
            new ShoppingCart { CustomerId = 3, DateCreated = DateTime.Today }
        };
        _context.ShoppingCarts.AddRange(shoppingCarts);

        var products = new List<Product>
        {
            new Product { ProdCatId = 1, Description = "Test Product 1", Stock = 10, BuyPrice = 100m, SellPrice = 150m },
            new Product { ProdCatId = 1, Description = "Test Product 2", Stock = 15, BuyPrice = 200m, SellPrice = 250m }
        };
        _context.Products.AddRange(products);

        var cartItems = new List<CartItem>
        {
            new CartItem { CartId = 1, ProductId = 1, Quantity = 2, Price = 150m },
            new CartItem { CartId = 1, ProductId = 2, Quantity = 1, Price = 250m },
            new CartItem { CartId = 2, ProductId = 1, Quantity = 1, Price = 150m }
        };
        _context.CartItems.AddRange(cartItems);

        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task TestCreateShoppingCart()
    {
        // Arrange
        var newCart = new ShoppingCart
        {
            CartId = 4, // Explicitly set a new ID
            CustomerId = 4,
            DateCreated = DateTime.Today
        };

        // Act
        await _shoppingCartRepository.Create(newCart);

        // Assert
        var savedCart = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => c.CartId == 4);

        Assert.NotNull(savedCart);
        Assert.Equal(newCart.CustomerId, savedCart.CustomerId);
        Assert.Equal(newCart.DateCreated, savedCart.DateCreated);
    }

    [Fact]
    public async Task TestGetShoppingCart()
    {
        // Act
        var cart1 = await _shoppingCartRepository.Find(1);
        var cart2 = await _shoppingCartRepository.Find(7);

        // Assert existing cart
        Assert.NotNull(cart1);
        Assert.Equal(2, cart1.CartItems.Count);
        Assert.Equal(1, cart1.CustomerId);
        
        // Assert non-existent cart
        Assert.Null(cart2);
    }

    [Fact]
    public async Task TestGetShoppingCarts()
    {
        // Act
        var carts = await _shoppingCartRepository.Read();

        // Assert
        Assert.NotNull(carts);
        Assert.Equal(3, carts.Count);
    }
    
    [Fact]
    public async Task TestUpdateShoppingCart()
    {
        // Arrange
        var cart = await _context.ShoppingCarts
            .Include(s => s.CartItems)
            .FirstOrDefaultAsync(s => s.CartId == 1);

        // Initial Assert
        Assert.NotNull(cart);
        Assert.Equal(DateTime.Today, cart.DateCreated);

        // Act
        cart.DateCreated = DateTime.Today.AddDays(2);
        var result = await _shoppingCartRepository.Update(cart);

        // Assert
        Assert.True(result);
        var updatedCart = await _context.ShoppingCarts
            .FirstOrDefaultAsync(s => s.CartId == 1);
        Assert.Equal(DateTime.Today.AddDays(2), updatedCart?.DateCreated);
    }

    [Fact]
    public async Task TestDeleteShoppingCart()
    {
        // Act
        var result1 = await _shoppingCartRepository.Delete(1);
        var result2 = await _shoppingCartRepository.Delete(2);
        var result3 = await _shoppingCartRepository.Delete(3);

        // Assert carts containing cart items
        Assert.False(result1);
        var deletedCart1 = await _context.ShoppingCarts.FindAsync(1);
        Assert.NotNull(deletedCart1);
        
        Assert.False(result2);
        var deletedCart2 = await _context.ShoppingCarts.FindAsync(2);
        Assert.NotNull(deletedCart2);
        
        // Assert empty cart
        Assert.True(result3);
        var deletedCart3 = await _context.ShoppingCarts.FindAsync(3);
        Assert.Null(deletedCart3);
    }
}
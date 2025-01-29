using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DAL;
using pbadraH60Services.Models;

namespace pbadraH60ServicesTest;

public class TestCrudCartItem : IDisposable
{
    private readonly ICartItemRepository<CartItem> _cartItemRepository;
    private readonly H60assignment2DbPbadraContext _context;

    public TestCrudCartItem()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<H60assignment2DbPbadraContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        
        _context = new H60assignment2DbPbadraContext(options);
        _cartItemRepository = new CartItemRepository(_context);
        
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

        var productCategories = new List<ProductCategory>
        {
            new ProductCategory() { ProdCat = "Test Category" },
        };
        _context.ProductCategories.AddRange(productCategories);

        _context.SaveChanges();
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async void TestGetCartItem()
    {
        // Act
        var cartItem1 = await _cartItemRepository.Find(1);
        var cartItem2 = await _cartItemRepository.Find(7);

        // Assert existing cart item
        Assert.NotNull(cartItem1);
        Assert.Equal(150m, cartItem1.Price);
        Assert.Equal(1, cartItem1.CartId);
        
        // Assert non-existent cart item
        Assert.Null(cartItem2);
    }

    [Fact]
    public async void TestGetCartItems()
    {
        // Act
        var cartItems = await _cartItemRepository.Read();

        // Assert
        Assert.NotNull(cartItems);
        Assert.Equal(3, cartItems.Count);
    }

    [Fact]
    public async void TestUpdateCartItem()
    {
        // Arrange
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(s => s.CartItemId == 1);

        // Initial Assert
        Assert.NotNull(cartItem);
        Assert.Equal(2, cartItem.Quantity);
        Assert.Equal(150m, cartItem.Price);

        // Act
        var updateCartItem = new CartItem 
        { 
            CartItemId = 1, 
            CartId = 1, 
            ProductId = 1, 
            Quantity = 4, 
            Price = 150m 
        };
        
        var result = await _cartItemRepository.Update(updateCartItem);

        // Assert
        Assert.True(result);
        var updatedCartItem = await _context.CartItems
            .FirstOrDefaultAsync(s => s.CartItemId == 1);
        
        Assert.Equal(150m, updatedCartItem?.Price);
        Assert.Equal(4, updatedCartItem?.Quantity);
    }

    [Fact]
    public async void TestDeleteCartItem()
    {
        // Act
        var result = await _cartItemRepository.Delete(1);

        // Assert
        Assert.True(result);
        
        // Assert shopping cart containing cartitem is not deleted alongside
        var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CartId == 1);
        Assert.NotNull(shoppingCart);
        
        var deletedCartItem = await _context.CartItems.FindAsync(1);
        Assert.Null(deletedCartItem);
    }

    [Fact]
    public async void TestCreateCartItem()
    {
        // Arrange
        var newCartItem = new CartItem()
        {
            CartId = 3,
            ProductId = 2,
            Price = 300m,
            Quantity = 2
        };

        // Act
        await _cartItemRepository.Create(newCartItem);

        // Assert
        var savedCartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.CartId == 3);

        Assert.NotNull(savedCartItem);
        Assert.Equal(newCartItem.Price, savedCartItem.Price);
        Assert.Equal(newCartItem.Quantity, savedCartItem.Quantity);
    }
}
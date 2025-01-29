using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.DAL;

public class OrderItemRepository : IOrderItemRepository<OrderItemDTO>
{
    private readonly H60assignment2DbPbadraContext _context;

    public OrderItemRepository(H60assignment2DbPbadraContext context)
    {
        _context = context;
    }

    public async Task<bool> Create(OrderItemDTO newOrderItem)
    {
        try
        {
            var orderItem = new OrderItem()
            {
                ProductId = newOrderItem.ProductId,
                OrderId = newOrderItem.OrderId,
                OrderItemId = newOrderItem.OrderItemId,
                Quantity = newOrderItem.Quantity,
                Price = newOrderItem.Price,
            };
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();

            var order = _context.Orders.FirstOrDefault(o => o.OrderId == newOrderItem.OrderId);
            var cartItem = _context.CartItems.Include(cI => cI.ShoppingCart).FirstOrDefault(cI =>
                cI.ProductId == newOrderItem.ProductId && cI.ShoppingCart.CustomerId == order.CustomerId);
            
            if (cartItem != null)
                _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<OrderItemDTO>> Read()
    {
        return await _context.OrderItems.Select(oI => new OrderItemDTO(oI)).ToListAsync();
    }

    public async Task<bool> Update(OrderItemDTO updatedOrderItem)
    {
        try
        {
            var existingOrderItem = await _context.OrderItems.FindAsync(updatedOrderItem.OrderItemId);
            if (existingOrderItem == null) return false;

            existingOrderItem.ProductId = updatedOrderItem.ProductId;
            existingOrderItem.OrderId = updatedOrderItem.OrderId;
            existingOrderItem.OrderItemId = updatedOrderItem.OrderItemId;
            existingOrderItem.Quantity = updatedOrderItem.Quantity;
            existingOrderItem.Price = updatedOrderItem.Price;

            _context.OrderItems.Update(existingOrderItem);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<OrderItemDTO> Find(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null) return null;
        return new OrderItemDTO(orderItem);
    }

    public async Task<List<OrderItemDTO>> FindByOrderId(int id)
    {
        return await _context.OrderItems.Where(oI => oI.OrderId == id).Select(oI => new OrderItemDTO(oI)).ToListAsync();
    }
}
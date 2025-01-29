using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.DAL;

public class OrderRepository : IOrderRepository<OrderDTO>
{
    private readonly H60assignment2DbPbadraContext _context;

    public OrderRepository(H60assignment2DbPbadraContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDTO>> ReadByDate(DateTime date)
    {
        return await _context.Orders.Include(o => o.Customer).Where(o => o.DateFulfilled == date).Select(o => new OrderDTO(o)).ToListAsync();
    }

    public async Task<List<OrderDTO>> ReadByCustomerName(string customerEmail)
    {
        var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
        
        return orders
            .Where(o => o.Customer.Email.ToLower() == customerEmail.ToLower())
            .Select(o => new OrderDTO(o)).ToList();
    }

    public async Task<List<OrderDTO>> Read()
    {
        return await _context.Orders.Include(o => o.Customer).Select(o => new OrderDTO(o)).ToListAsync();
    }

    public async Task<List<OrderDTO>> ReadByCustomerId(int customerId)
    {
        var order = await _context.Orders.Include(o => o.Customer).Where(o => o.CustomerId == customerId).Select(o => new OrderDTO(o)).ToListAsync();
        return order;
    }

    public async Task<bool> Create(OrderDTO orderDTO)
    {
        try
        {
            var order = new Order()
            {
                OrderId = orderDTO.OrderId,
                CustomerId = orderDTO.CustomerId,
                DateCreated = orderDTO.DateCreated,
                DateFulfilled = orderDTO.DateFulfilled,
                Total = orderDTO.Total,
                Taxes = orderDTO.Taxes,
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Update(OrderDTO orderDTO)
    {
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(s => s.OrderId == orderDTO.OrderId);

        if (existingOrder == null) return false;

        try
        {
            existingOrder.OrderId = orderDTO.OrderId;
            existingOrder.CustomerId = orderDTO.CustomerId;
            existingOrder.DateCreated = orderDTO.DateCreated;
            existingOrder.DateFulfilled = orderDTO.DateFulfilled;
            existingOrder.Total = orderDTO.Total;
            existingOrder.Taxes = orderDTO.Taxes;
            _context.Orders.Update(existingOrder);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<OrderDTO> Find(int id)
    {
        var order = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null) return null;
        return new OrderDTO(order);
    }
}
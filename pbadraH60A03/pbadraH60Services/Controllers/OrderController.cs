using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.DAL;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IOrderRepository<OrderDTO> _orderRepository;

    public OrderController(IOrderRepository<OrderDTO> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDTO>>> GetOrders()
    {
        return Ok(await _orderRepository.Read());
    }
    
    [HttpGet("Date/{dateFulfilled}")]
    public async Task<ActionResult<List<OrderDTO>>> GetOrdersByDateFulfilled(DateTime dateFulfilled)
    {
        return Ok(await _orderRepository.ReadByDate(dateFulfilled));
    }

    [HttpGet("Customer/{customerEmail}")]
    public async Task<ActionResult<List<OrderDTO>>> GetOrdersByCustomerName(string customerEmail)
    {
        var orders = await _orderRepository.ReadByCustomerName(customerEmail);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var order = await _orderRepository.Find(id);
        if (order == null) return NotFound();
        
        return Ok(order);
    }

    [HttpGet("Customer/CustomerId/{id}")]
    public async Task<ActionResult<List<OrderDTO>>> GetOrdersByCustomerId(int id)
    {
        return Ok(await _orderRepository.ReadByCustomerId(id));
    }

    [HttpPost]
    public async Task<ActionResult> PostOrder(OrderDTO order)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await _orderRepository.Create(order)) return Conflict("An error occurred");
        
        return CreatedAtAction(nameof(PostOrder), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutOrder(int id, OrderDTO order)
    {
        var existingOrder = await _orderRepository.Find(id);
        
        if (existingOrder == null) return NotFound();
        
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await _orderRepository.Update(order)) return Conflict("An error occurred");
        
        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.DAL;
using pbadraH60Services.DTO;

namespace pbadraH60Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : Controller
{
    private readonly IOrderItemRepository<OrderItemDTO> _orderItemRepository;

    public OrderItemController(IOrderItemRepository<OrderItemDTO> orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<OrderItemDTO>>> GetOrderItems()
    {
        return Ok(await _orderItemRepository.Read());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItemDTO>> GetOrderItem(int id)
    {
        var orderItem = await _orderItemRepository.Find(id);
        if (orderItem == null) return NotFound();
        
        return Ok(orderItem);
    }

    [HttpGet("Order/{orderId}")]
    public async Task<ActionResult<List<OrderItemDTO>>> GetOrderItems(int orderId)
    {
        var orderItems = await _orderItemRepository.FindByOrderId(orderId);
        if (orderItems == null) return NotFound();
        
        return Ok(orderItems);
    }

    [HttpPost]
    public async Task<ActionResult> PostOrderItem(OrderItemDTO orderItem)
    {
        if (!ModelState.IsValid) return BadRequest();

        if (!await _orderItemRepository.Create(orderItem)) return Conflict("An error occured");

        return CreatedAtAction(nameof(PostOrderItem), new { id = orderItem.OrderItemId }, orderItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutOrderItem(int id, OrderItemDTO orderItem)
    {
        var existingOrderItem = await _orderItemRepository.Find(id);
        if (existingOrderItem == null) return NotFound();
        
        if (!ModelState.IsValid) return BadRequest();
        
        if (!await _orderItemRepository.Update(orderItem)) return Conflict("An error occured");

        return Ok();
    }
}
using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController : BaseApiController 
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        // We need buyer email address from the token so that we can attach it to order,
        // we can't use the UserManager extension because we're not accessing user manager at
        // this point and we're not going to bring in use manager either to this controller.
        var email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();
        
        // Dto: input come from the client.
        var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

        var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

        if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

        return Ok(order);
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {
        var email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();
        
        var orders = await _orderService.GetOrdersForUserAsync(email);

        return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
    }   
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
    {
        var email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();
        
        var order = await _orderService.GetOrderByIdAsync(id, email);

        if (order == null) return NotFound(new ApiResponse(404));

        return order;
    }

    // Client can use this in the checkout system.
    [HttpGet("deliveryMethods")]
    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
    {
        return await _orderService.GetDeliveryMethodAsync();
    }


    
}
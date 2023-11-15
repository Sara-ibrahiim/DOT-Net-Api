using ApiDemo.HandelResponse;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.OrderServices;
using Services.Services.OrderServices.Dto;
using System.Security.Claims;

namespace ApiDemo.Controllers
{
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices) 
        {
            _orderServices = orderServices;
        }

        [HttpPost]
       public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto orderDto) 
        {
            //var email = User?.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.CreateOrderAsync(orderDto);
            if (order is null)
                return BadRequest(new ApiResponse(400, "Error while creating your order !! "));

            return Ok(order);
         

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrdersForUserAsync(string buyerEmail)

        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetAllOrdersForUserAsync(email);
            if (orders is { Count: <= 0 })
                return Ok(new ApiResponse(200, "You don't any orders yet"));
            return Ok(orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrdersByIdAsync(int id)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.GetOrdersByIdAsync(id ,email);

            if (order is null)
                return Ok(new ApiResponse(200, $"There is no Orderd with this Id {id}"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodAsync()
        
            =>Ok(await _orderServices.GetAllDeliveryMethodAsync());

        


    }
}

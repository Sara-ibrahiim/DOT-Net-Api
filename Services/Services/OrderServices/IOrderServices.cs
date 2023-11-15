using Core.Entities;
using Services.Services.OrderServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderServices
{
    public interface IOrderServices
    {
        Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto);
        Task <IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail);
        Task<OrderResultDto> GetOrdersByIdAsync(int id ,string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();

    }
}

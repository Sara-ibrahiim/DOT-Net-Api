using Services.Services.BasketService.Dto;
using Services.Services.OrderServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntenId);

        Task <OrderResultDto> UpdateOrderPaymentFailed(string paymentIntenId);
    }
}

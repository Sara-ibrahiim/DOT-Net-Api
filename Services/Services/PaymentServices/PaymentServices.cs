using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Sepecifcations;
using Microsoft.Extensions.Configuration;
using Services.Services.BasketService;
using Services.Services.BasketService.Dto;
using Services.Services.OrderServices.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Services.Services.PaymentServices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUntiOfWork _untiOfWork;
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentServices( 
            IUntiOfWork untiOfWork,
            IBasketService basketService,
            IConfiguration configuration,
            IMapper mapper
            
            
            ) 
        {
            _untiOfWork = untiOfWork;
            _basketService = basketService;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Strip:SecretKey"];
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket == null)
                return null;

            var shippingPrice = 0m;
            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _untiOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);    
                shippingPrice= deliveryMethod.Price;    
            }
            foreach(var item in basket.BasketItems)
            {
                var producteItem = await _untiOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if(item.Price!=producteItem.Price)
                    item.Price = producteItem.Price;    

            }

            var service = new PaymentIntentService();

            PaymentIntent intent;


            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity *(item.Price * 100 ))+ ((long)shippingPrice * 100),
                     Currency = "usd",
                     PaymentMethodTypes = new List<string> { "crad"}

                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;


            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100)
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketService.UpdateBasketAsync(basket);


            return basket;


        }


        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntenId)
        {
            var specs = new OrderWithPaymentIntentSepecifcations(paymentIntenId);
                var order = await _untiOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);
            if (order is null)

                return null;

            order.OrderStatus = OrderStatus.PaymentFailed;
            _untiOfWork.Repository<Order>().Update(order);
            await _untiOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;

        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntenId)
        {
            var specs = new OrderWithPaymentIntentSepecifcations(paymentIntenId);
            var order = await _untiOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);
            if (order is null)

                return null;

            order.OrderStatus = OrderStatus.PaymentReceived;
            _untiOfWork.Repository<Order>().Update(order);
            await _untiOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}

using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Sepecifcations;
using Microsoft.EntityFrameworkCore.Metadata;
using Services.Services.BasketService;
using Services.Services.OrderServices.Dto;
using Services.Services.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderServices
{
    public class OrderServices : IOrderServices

    {
        private readonly IBasketService _basketService;
        private readonly IUntiOfWork _untiOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public OrderServices(IBasketService basketService , IUntiOfWork untiOfWork , IMapper mapper , IPaymentServices paymentServices
        )
        {
         _basketService = basketService;
        _untiOfWork = untiOfWork;
            _mapper = mapper;
            _paymentServices = paymentServices;
        }
        async Task<OrderResultDto> IOrderServices.CreateOrderAsync(OrderDto orderDto)
        {
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                return null;
            var orderItems = new List<OrderItemDto>();
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _untiOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(productItem.Price, item.Quantity, itemOrdered);


                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }
            var deliveryMethod = await _untiOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var specs = new OrderWithPaymentIntentSepecifcations(basket.PaymentIntentId);

            var existingorder = await _untiOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);
            //var specs = new OrderWithItemsSepecifcations(basket)
             if (existingorder != null)
            {
                _untiOfWork.Repository<Order>().Delete(existingorder);
                await _paymentServices.CreateOrUpdatePaymentIntent(orderDto.BasketId);

            }
            var mappedShipingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);



            var order = new Order(orderDto.BuyerEmail, mappedShipingAddress, deliveryMethod, mappedOrderItems, subTotal,basket.PaymentIntentId);
            await _untiOfWork.Repository<Order>().Add(order);

            await _untiOfWork.Complete();

            await _basketService.DeleteBasketAsync(orderDto.BasketId);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;

        }
        public  async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
     => await _untiOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSepecifcations(buyerEmail);
            var orders = await _untiOfWork.Repository<Order>().GetAllWithSpecificationsAsync(specs);

            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrdersByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSepecifcations(id ,buyerEmail);
            var order = await _untiOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            var mappedOrders = _mapper.Map<OrderResultDto>(order);

            return mappedOrders;
        }

      
    }
}

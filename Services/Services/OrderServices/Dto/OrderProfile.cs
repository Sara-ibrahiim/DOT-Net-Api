using AutoMapper;
using Core.Entities.OrderEntities;
using Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderServices.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {
            CreateMap< Address,AddressDto>().ReverseMap();
            CreateMap< AddressDto, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.DeliveryMethod, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
          .ForMember(dest => dest.ProductItemId, option => option.MapFrom(src => src.ItemOrdered.ProductItemId))
           .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ItemOrdered.ProductName))
          .ForMember(dest => dest.PictureUrll, option => option.MapFrom(src => src.ItemOrdered.PicturUrl))
           .ForMember(dest => dest.PictureUrll, option => option.MapFrom<OrderItemResolver>());

            
        }
    }
}

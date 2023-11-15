using Infrastructure.Interfaces;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Services.Services.ProductSerives.Dto;
using Services.Services.ProductSerives;
using ApiDemo.HandelResponse;
using Services.Services.CacheService;
using Services.Services.BasketService.Dto;
using Infrastructure.BasketRepository;
using Services.Services.BasketService;
using Services.Services.TokenServices;
using Services.Services.UserService;
using Services.Services.OrderServices.Dto;
using Services.Services.PaymentServices;
using Services.Services.OrderServices;

namespace ApiDemo.Extensions
{
    public  static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService (this IServiceCollection services)
        {
          services.AddScoped<IProuductRepository, ProuductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           services.AddScoped<IUntiOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService,CacheService >();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentServices, PaymentServices>();
            services.AddScoped<IOrderServices, OrderServices>();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(model => model.Value.Errors.Count > 0)
                    .SelectMany(model => model.Value.Errors)
                    .Select(error => error.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);

                };

            });
            
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));
            
            return services;

        }
    }
}

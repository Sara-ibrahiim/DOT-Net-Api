using ApiDemo.HandelResponse;
using Core.Entities.OrderEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.BasketService.Dto;
using Services.Services.OrderServices.Dto;
using Services.Services.PaymentServices;
using Stripe;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly ILogger<PaymentController> _logger;
        private const string WhSecret = "whsec_8ea3685711217a51cbb830a2ceffd6132c8102034ef89c89619b4a40a99bf99e";

        public PaymentController(IPaymentServices paymentServices , ILogger<PaymentController> logger)
        {
           _paymentServices = paymentServices;
           _logger = logger;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if ( basket is null)
                BadRequest(new ApiResponse(400, "Problem With Your Basket!! "));
            return Ok(basket);


        }
        //adorer-nobly-geeky-salute
         [HttpPost]
         public async Task<ActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"], WhSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", paymentIntent.Id);
                    order = await _paymentServices.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Update To Payment Failed:", order.Id);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", paymentIntent.Id);
                    order = await _paymentServices.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Update To Payment Succeeded:", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}

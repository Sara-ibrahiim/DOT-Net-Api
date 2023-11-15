using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Services.CacheService;
using System.Text;

namespace ApiDemo.Helper
{
    public class CacheAttribute : Attribute , IAsyncActionFilter
    {
        private readonly int _timeTolineInSeconds;

        public CacheAttribute(int timeTolineInSeconds) 
        {
            _timeTolineInSeconds = timeTolineInSeconds;
        }

        //public async Task OnActionExecutionAsync(ActionExecutedContext context, ActionExecutionDelegate next)
        //{
        //    var cacheservice = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();


        //    var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);


        //    var cachedResponse = await cacheservice.GetCacheResponseAsync(cacheKey);

        //    if (!string.IsNullOrEmpty(cachedResponse))
        //    {
        //        var contentResult = new ContentResult
        //        {
        //            Content = cachedResponse,
        //            ContentType = "application/json",
        //            StatusCode = 200
        //        };

        //        context.Result = contentResult;

        //        return;
        //    }

        //    var executedContext = await next();


        //    if (executedContext.Result is OkObjectResult response)
        //        await cacheservice.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeTolineInSeconds));
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheservice = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();


            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);


            var cachedResponse = await cacheservice.GetCacheResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();


            if (executedContext.Result is OkObjectResult response)
                await cacheservice.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeTolineInSeconds));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {

            var cachekey = new StringBuilder();
            cachekey.Append($"{request.Path}");

            foreach ( var (key, value) in request.Query.OrderBy (x => x.Key))
            {
                cachekey.Append($"{key}-{value}");

            }
            return cachekey.ToString();
        }
    }
}

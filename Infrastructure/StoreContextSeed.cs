using Core;
using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.ProductBrands != null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands is not null)
                    {
                        foreach (var brand in brands)

                            await context.ProductBrands.AddAsync(brand);

                        await context.SaveChangesAsync();

                    }

                }
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null)
                    {
                        foreach (var type in types)

                            await context.ProductTypes.AddAsync(type);

                        await context.SaveChangesAsync();
                    }



                }
                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products is not null)
                    {
                        foreach (var product in products)

                            await context.Products.AddAsync(product);

                        await context.SaveChangesAsync();
                    }


                }


                if (context.DeliveryMethod != null && !context.DeliveryMethod.Any())
                {
                    var deliveryMethodData = File.ReadAllText("../Infrastructure/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

                    if (deliveryMethods is not null)
                    {
                        foreach (var deliveryMethod in deliveryMethods)

                            await context.DeliveryMethod.AddAsync(deliveryMethod);

                        await context.SaveChangesAsync();
                    }


                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}

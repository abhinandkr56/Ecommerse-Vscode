
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class SoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if(!context.ProductBrands.Any())
                {
                    var brandsData=File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach(var item in brands){
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                 if(!context.ProductTypes.Any())
                {
                    var typesData=File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types=JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach(var item in types){
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                 if(!context.Products.Any())
                {
                    var productsdata=File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var product=JsonSerializer.Deserialize<List<Product>>(productsdata);
                    foreach(var item in product){
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex){
                var logger=loggerFactory.CreateLogger<StoreContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}
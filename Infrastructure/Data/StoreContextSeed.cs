using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
/*
csak adatokat olvasunk ki jsonből és ezekkel töltjük fel az adatbázisunkat

*/
namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.ProductBrands.Any()) {
                //ha nincs semmilyen adat az adatbázisban akkor ...
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brand);
            }
            
            if(!context.ProductTypes.Any()) {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }

            if(!context.Products.Any()) {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }

            //ha bármilyen változás történik akkor mentünk
            if (context.ChangeTracker.HasChanges()) {
                await context.SaveChangesAsync();
            }
        }
    }
}
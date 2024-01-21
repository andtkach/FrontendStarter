using BFF.Entities;

namespace BFF.Data;

public static class DbInitializer
{
    public static async Task Initialize(DataContext context)
    {
        if (context.Products.Any()) return;

        var products = new List<Product>
            {
                new Product
                {
                    Name = "Test 1",
                    Description =
                        "Lorem ipsum dolor sit amet.",
                },
            };

        foreach (var product in products)
        {
            context.Products.Add(product);
        }

        await context.SaveChangesAsync();
    }
}
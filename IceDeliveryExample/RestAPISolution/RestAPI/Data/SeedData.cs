using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestAPI.Model;
using RestAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPI.Data
{
    internal class SeedData
    {
        internal void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                var orders = new List<Order>();
                for (int i = 1; i <= 15; i++)
                {
                    orders.Add(new Order
                    {
                        CustomerName = $"Customer {i}",
                        DeliveryAddress = $"Address {i}",
                        QuantityInKg = i * 10,
                        OrderDate = DateTime.UtcNow.AddDays(-i)
                    });
                }

                db.Orders.AddRange(orders);
                db.SaveChanges();
            }
        }
    }
}

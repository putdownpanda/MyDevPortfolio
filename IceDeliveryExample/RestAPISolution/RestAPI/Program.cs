using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestAPI.Data;
using RestAPI.Model;
using RestAPI.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseInMemoryDatabase("IceDeliveryDb"));

var app = builder.Build();

// Seed the database
SeedData seed = new SeedData();
seed.SeedDatabase(app);

app.MapOpenApi();

// Define API Endpoints
app.MapPost("/orders", async (OrderRequest request, OrderDbContext db) =>
{
    var order = new Order
    {
        CustomerName = request.CustomerName,
        DeliveryAddress = request.DeliveryAddress,
        QuantityInKg = request.QuantityInKg,
        OrderDate = DateTime.UtcNow
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
})
.WithSummary("Create a new order")
.WithName("CreateOrder")
.WithDescription("Creates a new order for ice delivery")
.WithOpenApi();

app.MapGet("/orders", async (OrderDbContext db) =>
{
    return Results.Ok(await db.Orders.ToListAsync());
})
.WithSummary("Get all orders")
.WithName("GetOrders")
.WithDescription("Retrieves all orders for ice delivery")
.WithOpenApi();

app.MapGet("/orders/{id}", async (int id, OrderDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);

    return order is not null ? Results.Ok(order) : Results.NotFound();
})
.WithSummary("Get order by Id")
.WithName("GetOrderById")
.WithSummary("Retrieves an order by its Id")
.WithOpenApi();

app.MapDelete("/orders/{id}", async (int id, OrderDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);

    if (order is not null)
    {
        db.Orders.Remove(order);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
})
.WithSummary(summary: "Delete order by Id")
.WithName("DeleteOrder")
.WithDescription("Deletes an order by its Id")
.WithOpenApi();

app.Run();

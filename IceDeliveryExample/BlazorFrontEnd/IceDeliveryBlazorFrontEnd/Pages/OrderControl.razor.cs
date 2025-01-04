using IceDeliveryBlazorFrontEnd.Services.API;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Model;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Radzen.Blazor;

namespace IceDeliveryBlazorFrontEnd.Pages;

public partial class OrderControl
{
    protected RadzenDataGrid<Order> orderGrid;
    protected IEnumerable<Order> orderData;

    [Inject]
    private IceDeliveryCalls iceDeliverCalls { get; set; }
    protected override async Task OnInitializedAsync()
    {
        StartUp();
        await UpdateOrders();
    }

    private async Task UpdateOrders()
    {
        orderData = await iceDeliverCalls.Orders(new OrderRequest());
        //betTypesData = Context.wpp_Product_Catalogues;
    }
}
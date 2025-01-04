using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Model;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.PostRequests;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Request;
using System.Text.Json;

namespace IceDeliveryBlazorFrontEnd.Services.API;

public class IceDeliveryCalls(IceDeliveryService sService, ILogger<IceDeliveryCalls> logger)
{
    public async Task<string> Anon(object request)
    {
        //Meeting meetingResponse = new Meeting();
        string resp = string.Empty;
        try
        {
            HttpResponseMessage response = await sService.Anon(request);

            if (response.IsSuccessStatusCode)
            {
                resp = await response.Content.ReadAsStringAsync();
                //meetingResponse = JsonSerializer.Deserialize<Meeting>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                logger.LogInformation($"Meeting receieved.");
            }
            else
            {
                logger.LogError($"Failed to retrieve Meeting: HTTP {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception during Meeting check: {ex.Message}");
        }
        return resp;
    }

    public async Task<List<Order>> Orders(OrderRequest request)
    {
        List<Order> scheduleResponse = new List<Order>();
        try
        {
            HttpResponseMessage response = await sService.AllOrders(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                scheduleResponse = JsonSerializer.Deserialize<List<Order>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                logger.LogInformation($" receieved.");

            }
            else
            {
                logger.LogError($"Failed to retrieve : HTTP {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception during  check: {ex.Message}");

        }

        return scheduleResponse;
    }
    public async Task<Order> GetOrder(Order request)
    {
        Order scheduleResponse = new Order();
        try
        {
            HttpResponseMessage httpResponse = await sService.OrderDetails(request);

            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                scheduleResponse = JsonSerializer.Deserialize<Order>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                logger.LogInformation($" receieved.");

            }
            else
            {
                logger.LogError($"Failed to retrieve : HTTP {httpResponse.StatusCode} - {await httpResponse.Content.ReadAsStringAsync()}");

            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception during  check: {ex.Message}");

        }

        return scheduleResponse;
    }
    public async Task<Order> UpdateOrder(OrderPostRequest request)
    {
        var response = new Order();
        try
        {
            HttpResponseMessage httpResponse = await sService.UpdateOrder(request);

            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<Order>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                logger.LogInformation($"receieved.");
            }
            else
            {
                logger.LogError($"Failed to retrieve: HTTP {httpResponse.StatusCode} - {await httpResponse.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception during check: {ex.Message}");

        }
        return response;
    }


}
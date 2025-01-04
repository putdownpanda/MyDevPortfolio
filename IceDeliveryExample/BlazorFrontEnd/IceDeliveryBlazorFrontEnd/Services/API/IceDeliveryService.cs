using IceDeliveryBlazorFrontEnd.Services.GET;
using IceDeliveryBlazorFrontEnd.Services.HTTPClientName;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.ApiPath;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Model;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.PostRequests;
using IceDeliveryBlazorFrontEnd.Services.IceDelivery.Request;
using IceDeliveryBlazorFrontEnd.Services.POST;

namespace IceDeliveryBlazorFrontEnd.Services.API;

public class IceDeliveryService(
    IHttpClientFactory httpClientFactory,
    ILogger<IceDeliveryCalls> logger,
    IHttpContextAccessor context)
    : Service
{
    private readonly GET_Async<object> _getRequester = new(httpClientFactory, logger);
    private readonly POST_Async<object> _getPoster = new(httpClientFactory, logger, context);
    private Dictionary<Type, string> _requestToApiPathMapping;
    private void InitializeRequestToApiPathMapping()
    {
        _requestToApiPathMapping = new Dictionary<Type, string>
        {
            { typeof(OrderRequest), APIPath.RetrieveOrder },
            { typeof(OrderPostRequest), APIPath.PlaceOrder },
        };
    }
    private string GetApiPathForRequest(object request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        Type requestType = request.GetType();

        if (_requestToApiPathMapping.TryGetValue(requestType, out string apiPath))
        {
            return apiPath;
        }
        else
        {
            throw new KeyNotFoundException($"No API path found for request type {requestType.Name}.");
        }
    }
    public override async Task<HttpResponseMessage> Anon(object request)
    {
        InitializeRequestToApiPathMapping();
        return await _getRequester.GetRequestAsync(APIPath.Root, GetApiPathForRequest(request), HttpClientLabel.IceDeliveryAPI, request);
    }
    public async Task<HttpResponseMessage> AllOrders(OrderRequest request)
    {
        return await _getRequester.GetRequestAsync(APIPath.Root, APIPath.RetrieveAllOrders, HttpClientLabel.IceDeliveryAPI, request);
    }
    public async Task<HttpResponseMessage> OrderDetails(Order request)
    {
        return await _getRequester.GetRequestAsync(APIPath.Root, APIPath.RetrieveOrder, HttpClientLabel.IceDeliveryAPI, request);
    }
    public async Task<HttpResponseMessage> UpdateOrder(OrderPostRequest request)
    {
        return await _getPoster.PostRequestAsync(APIPath.Root, APIPath.PlaceOrder, HttpClientLabel.IceDeliveryAPI, "", request, "");
    }
}
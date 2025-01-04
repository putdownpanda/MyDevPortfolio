using System.Net.Http.Headers;

namespace IceDeliveryBlazorFrontEnd.Services.POST;

public class POST_Async<T>(
    IHttpClientFactory httpClientFactory,
    ILogger<T> logger,
    IHttpContextAccessor context)
{
    private HttpResponseMessage _responseMessage;

    public async Task<HttpResponseMessage> PostRequestAsync<T>(string root, string endpoint, string apiLabel, string scope, T? request, string? headerParameters)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(apiLabel);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");

            var url = $"{httpClient.BaseAddress}{root}{endpoint}{headerParameters}";
            Console.WriteLine($"Making Post request to: {url}");
            Console.WriteLine($"The Post is: {request.ToString()}");

            _responseMessage = await httpClient.PostAsJsonAsync(url, request);
            Console.WriteLine($"The Post is: {_responseMessage.Content.ReadAsStringAsync().Result}");

            if (!_responseMessage.IsSuccessStatusCode)
            {
                var apiError = ApiError.FromStatusCode((int)_responseMessage.StatusCode);
                return _responseMessage;
            }
            //format post httpcontext for auditing middleware
            context.HttpContext.Items.TryGetValue("request", out var existingRequest);
            if (existingRequest is not null)
            {
                context.HttpContext.Items["request"] = request;
            }
            else
            {
                context.HttpContext.Items.Add("request", request);
            }
            context.HttpContext.Request.Path = '/' + url;
            context.HttpContext.Request.Method = HttpMethods.Post;

            return _responseMessage;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "A critical error occurred during data processing: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
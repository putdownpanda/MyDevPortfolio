using System.Net.Http.Headers;
using System.Web;

namespace IceDeliveryBlazorFrontEnd.Services.GET;

public class GET_Async<T>(IHttpClientFactory httpClientFactory, ILogger<T> logger)
{
    public async Task<HttpResponseMessage> GetRequestAsync<T>(string root, string endpoint, string apiLabel, T request = default)
    {
        try
        {

            var httpClient = httpClientFactory.CreateClient(apiLabel);
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");


            // Convert the request object into query parameters only if a request object was provided
            if (request != null)
            {
                var queryParams = new Dictionary<string, string>();
                foreach (var property in request.GetType().GetProperties())
                {
                    var value = property.GetValue(request);
                    if (value != null)
                    {
                        queryParams[property.Name] = value.ToString();
                    }
                }

                // Add the query parameters to the endpoint URL
                if (queryParams.Count > 0)
                {
                    endpoint += "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
                }
            }


            var url = $"{httpClient.BaseAddress}{root}{endpoint}";
            Console.WriteLine($"Making GET request to: {url}");

            var responseMessage = await httpClient.GetAsync($"{url}");
            if (responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
            }
            else
            {
                var apiError = ApiError.FromStatusCode((int)responseMessage.StatusCode);
                logger.LogError($"API error: {apiError.Message} -- {apiError.StatusCode}");
                Console.WriteLine($"API error: {apiError.Message} -- {apiError.StatusCode}");
            }
            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //	var apiError = ApiError.FromStatusCode((int)responseMessage.StatusCode);
            //	return responseMessage;
            //}

            //if (responseMessage.Headers.ETag != null) // can use this later to not poll the same data, just pass the param in the header prior to the request
            //{
            //	_eTagStore.SaveETag(endpoint, responseMessage.Headers.ETag);
            //}

            return responseMessage;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "A critical error occurred during data processing: {ErrorMessage}", ex.Message);
            return null;
        }
    }
}
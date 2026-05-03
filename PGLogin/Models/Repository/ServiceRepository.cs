using Newtonsoft.Json;
using PGLogin.Models.Repository.IRepository;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PGLogin.Models.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly APIResponse _responseModel;

        public ServiceRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _responseModel = new APIResponse();
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MagicAPI");
                using var message = new HttpRequestMessage();

                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                message.RequestUri = new Uri(apiRequest.Url);

                // ✅ Add content if data exists
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                // ✅ Determine the HTTP method
                message.Method = apiRequest.ApiType?.ToUpper() switch
                {
                    "POST" => HttpMethod.Post,
                    "PUT" => HttpMethod.Put,
                    "DELETE" => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                // ✅ Add Authorization if token exists
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                // ✅ Send request
                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    // Try to deserialize as APIResponse
                    var parsedResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if (parsedResponse != null &&
                        (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                         apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))
                    {
                        parsedResponse.StatusCode = System.Net.HttpStatusCode.OK;
                        parsedResponse.IsSuccess = true;

                        var resultJson = JsonConvert.SerializeObject(parsedResponse.Result);
                        return JsonConvert.DeserializeObject<T>(resultJson)!;
                    }
                }
                catch
                {
                    // If not an APIResponse, return direct deserialization
                    return JsonConvert.DeserializeObject<T>(apiContent)!;
                }

                return JsonConvert.DeserializeObject<T>(apiContent)!;
            }
            catch (Exception ex)
            {
                // ✅ Handle any exception gracefully
                var errorResponse = new APIResponse
                {
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };

                var errorJson = JsonConvert.SerializeObject(errorResponse);
                return JsonConvert.DeserializeObject<T>(errorJson)!; 
            }
        }
    }
}

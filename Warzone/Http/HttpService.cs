using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models;

namespace Warzone.Http
{
    public class HttpService : IHttpService
    {
        private static HttpClient _httpClient;

        private readonly string _baseCookie;
            

        private const string UserAgent = "a4b471be-4ad2-47e2-ba0e-e1f2aa04bff9";

        public HttpService(string baseCookie)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Cookie", baseCookie);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", UserAgent);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        public async Task<HttpResponse<T>> GetAsync<T>(string resourceUrl, CancellationToken? cancellationToken = null)
            where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourceUrl);
            var response = cancellationToken.HasValue
                ? await _httpClient.SendAsync(request, cancellationToken.Value)
                : await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<T>
                {
                    Success = false,
                    Content = null,
                    Headers = response.Headers,
                    StatusCode = response.StatusCode
                };
            }

            var contents = cancellationToken.HasValue
                ? await response.Content.ReadAsStringAsync(cancellationToken.Value)
                : await response.Content.ReadAsStringAsync();

            var deserializedResponse = DeserializeResponseFromApi<T>(contents);

            return new HttpResponse<T>
            {
                Success = true,
                Content = deserializedResponse,
                Headers = response.Headers,
                StatusCode = response.StatusCode
            };
        }

        public async Task<HttpResponse<TResponse>> PostAsync<TResponse>(string resourceUrl, HttpContent content,
            CancellationToken? cancellationToken) where TResponse : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, resourceUrl) {Content = content};
            var response = cancellationToken.HasValue
                ? await _httpClient.SendAsync(request, cancellationToken.Value)
                : await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode || (int)response.StatusCode == 0)
            {
                return new HttpResponse<TResponse>
                {
                    Success = false,
                    Content = null,
                    Headers = response.Headers
                };
            }

            var contents = cancellationToken.HasValue
                ? await response.Content.ReadAsStringAsync(cancellationToken.Value)
                : await response.Content.ReadAsStringAsync();

            var deserializedResponse = DeserializeResponseFromApi<TResponse>(contents);

            return new HttpResponse<TResponse>()
            {
                Success = true,
                Content = deserializedResponse,
                Headers = response.Headers
            };
        }

        public void UpdateDefaultHeaders(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        private static T DeserializeResponseFromApi<T>(string responseContents) where T : class
        {
            T deserializedResponse;

            try
            {
                deserializedResponse = JsonSerializer.Deserialize<T>(responseContents, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException)
            {
                deserializedResponse = null;
            }

            return deserializedResponse;
        }
    }
}
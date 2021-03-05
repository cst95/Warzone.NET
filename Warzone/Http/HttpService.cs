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

        public HttpService()
        {
            _httpClient = new HttpClient();
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

            if (!response.IsSuccessStatusCode)
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

        private static T DeserializeResponseFromApi<T>(string responseContents) where T : class
        {
            T deserializedResponse;

            try
            {
                deserializedResponse = JsonSerializer.Deserialize<T>(responseContents);
            }
            catch (JsonException)
            {
                deserializedResponse = null;
            }

            return deserializedResponse;
        }
    }
}
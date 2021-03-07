using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models;

namespace Warzone.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _baseHeaders;

        public HttpService()
        {
            _httpClient = new HttpClient();
            _baseHeaders = new Dictionary<string, string>()
            {
                {
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36"
                },
                {
                    "X-Requested-With",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36"
                },
                {"Accept", "application/json"},
                {"Connection", "keep-alive"}
            };

            ResetDefaultHeaders();
        }

        public async Task<HttpResponse<T>> GetAsync<T>(string resourceUrl,
            Dictionary<string, string> headersToAdd = null, CancellationToken? cancellationToken = null)
            where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourceUrl);

            if (headersToAdd != null) AddHeaders(request, headersToAdd);

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
            Dictionary<string, string> headersToAdd = null,
            CancellationToken? cancellationToken = null) where TResponse : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, resourceUrl) {Content = content};

            if (headersToAdd != null) AddHeaders(request, headersToAdd);

            var response = cancellationToken.HasValue
                ? await _httpClient.SendAsync(request, cancellationToken.Value)
                : await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode && (int)response.StatusCode != 302)
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

        public void ResetDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");

            foreach (var (key, val) in _baseHeaders)
            {
                UpdateDefaultHeaders(key, val);
            }
        }

        public void UpdateDefaultHeaders(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }
        
        public void UpdateDefaultHeaders(string key, IEnumerable<string> value)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headersToAdd)
        {
            foreach (var (key, value) in headersToAdd)
            {
                request.Headers.Add(key, value);
            }
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
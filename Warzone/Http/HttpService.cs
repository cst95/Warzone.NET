using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models.CodApi;
using Warzone.Models.Http;

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

        public async Task<BaseHttpResponse> GetAsync(string resourceUrl,
            Dictionary<string, string> headersToAdd = null, CancellationToken? cancellationToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourceUrl);

            if (headersToAdd != null) AddHeaders(request, headersToAdd);

            var response = cancellationToken.HasValue
                ? await _httpClient.SendAsync(request, cancellationToken.Value)
                : await _httpClient.SendAsync(request);

            return CheckResponse(response);
        }

        public async Task<BaseHttpResponse> PostAsync(string resourceUrl, HttpContent content,
            Dictionary<string, string> headersToAdd = null,
            CancellationToken? cancellationToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, resourceUrl) {Content = content};

            if (headersToAdd != null) AddHeaders(request, headersToAdd);

            var response = cancellationToken.HasValue
                ? await _httpClient.SendAsync(request, cancellationToken.Value)
                : await _httpClient.SendAsync(request);

            return CheckResponse(response);
        }

        private BaseHttpResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode && (int) response.StatusCode != 302)
            {
                return new BaseHttpResponse
                {
                    Success = false,
                    Headers = response.Headers,
                    StatusCode = response.StatusCode,
                    ResponseContent = response.Content
                };
            }

            return new BaseHttpResponse
            {
                Success = true,
                Headers = response.Headers,
                StatusCode = response.StatusCode,
                ResponseContent = response.Content
            };
        }

        public async Task<HttpResponse<TResponse>> GetAsync<TResponse>(string resourceUrl,
            Dictionary<string, string> headersToAdd = null, CancellationToken? cancellationToken = null)
            where TResponse : class
        {
            var clientResponse =
                new HttpResponse<TResponse>(await GetAsync(resourceUrl, headersToAdd, cancellationToken));

            var contents = cancellationToken.HasValue
                ? await clientResponse.ResponseContent.ReadAsStringAsync(cancellationToken.Value)
                : await clientResponse.ResponseContent.ReadAsStringAsync();

            var deserializedResponse = DeserializeResponseFromApi<ResponseWrapper<TResponse>>(contents);

            if (deserializedResponse != null && !contents.Contains("\"status\":\"error\""))
                clientResponse.Content = deserializedResponse.Data;
            else
            {
                var error = DeserializeResponseFromApi<ResponseWrapper<Error>>(contents);
                clientResponse.Error = error.Data;
            }

            return clientResponse;
        }

        private void ResetDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");

            foreach (var (key, val) in _baseHeaders)
            {
                UpdateDefaultHeaders(key, val);
            }
        }

        public void UpdateDefaultHeaders(string key, IEnumerable<string> value)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        private void UpdateDefaultHeaders(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headersToAdd)
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
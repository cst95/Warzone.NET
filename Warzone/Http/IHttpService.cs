using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models.Http;

namespace Warzone.Http
{
    public interface IHttpService
    {
        Task<BaseHttpResponse> GetAsync(string resourceUrl,
            Dictionary<string, string> headersToAdd = null, CancellationToken? cancellationToken = null);

        Task<HttpResponse<TResponse>> GetAsync<TResponse>(string resourceUrl, Dictionary<string, string> headersToAdd,
            CancellationToken? cancellationToken) where TResponse : class;

        Task<BaseHttpResponse> PostAsync(string resourceUrl, HttpContent content,
            Dictionary<string, string> headersToAdd = null,
            CancellationToken? cancellationToken = null);

        Task<HttpResponse<TResponse>> PostAsync<TResponse>(string resourceUrl,
            HttpContent content,
            Dictionary<string, string> headersToAdd,
            CancellationToken? cancellationToken) where TResponse : class;

        void UpdateDefaultHeaders(string key, IEnumerable<string> value);
    }
}
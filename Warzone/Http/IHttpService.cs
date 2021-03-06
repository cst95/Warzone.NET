using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models;

namespace Warzone.Http
{
    public interface IHttpService
    {
        Task<HttpResponse<T>> GetAsync<T>(string resourceUrl, Dictionary<string, string> headersToAdd,
            CancellationToken? cancellationToken)
            where T : class;

        Task<HttpResponse<TResponse>> PostAsync<TResponse>(string resourceUrl,
            HttpContent content,
            Dictionary<string, string> headersToAdd,
            CancellationToken? cancellationToken) where TResponse : class;

        void UpdateDefaultHeaders(string key, string value);
        void UpdateDefaultHeaders(string key, IEnumerable<string> value);
        void ResetDefaultHeaders();
    }
}
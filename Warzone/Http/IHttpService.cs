using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models;

namespace Warzone.Http
{
    public interface IHttpService
    {
        Task<HttpResponse<T>> GetAsync<T>(string resourceUrl, CancellationToken? cancellationToken)
            where T : class;

        Task<HttpResponse<TResponse>> PostAsync<TResponse>(string resourceUrl, HttpContent content,
            CancellationToken? cancellationToken) where TResponse : class;
    }
}
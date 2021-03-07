using System.Threading;
using System.Threading.Tasks;
using Warzone.Models.CodApi;

namespace Warzone.Clients
{
    public interface ICodApiClient
    {
        Task<string> FetchXsrfTokenAsync(CancellationToken? cancellationToken = null);

        Task<ResponseWrapper<SummariesWrapper>> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);

        Task<bool> LoginAsync(string email, string password, string xsrfToken,
            CancellationToken? cancellationToken = null);
    }
}
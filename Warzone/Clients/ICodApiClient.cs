using System;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Models.CodApi;

namespace Warzone.Clients
{
    public interface ICodApiClient
    {
        Task<string> FetchXsrfTokenAsync(CancellationToken? cancellationToken = null);

        Task<bool> LoginAsync(string email, string password, string xsrfToken,
            CancellationToken? cancellationToken = null);

        Task<CodApiResponse<Summaries>> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);

        Task<CodApiResponse<Summaries>> GetWarzoneMatchesAsync(string playerName, string platform,
            DateTime? startTime, DateTime? endTime, CancellationToken? cancellationToken);
    }
}
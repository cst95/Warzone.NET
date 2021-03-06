using System.Threading;
using System.Threading.Tasks;

namespace Warzone.Clients
{
    public interface ICodApiClient
    {
        Task<object> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);
    }
}
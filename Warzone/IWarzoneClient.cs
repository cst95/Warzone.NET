using System.Threading;
using System.Threading.Tasks;
using Warzone.Models;
using Warzone.Models.CodApi;

namespace Warzone
{
    public interface IWarzoneClient
    {
        Task<WarzoneResponse<Summaries>> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);

        Task<bool> LoginAsync(string email, string password);
    }
}
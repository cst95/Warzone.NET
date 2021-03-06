using System.Threading;
using System.Threading.Tasks;

namespace Warzone
{
    public interface IWarzoneClient
    {
        Task<object> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);

        Task<bool> LoginAsync(string email, string password);
    }
}
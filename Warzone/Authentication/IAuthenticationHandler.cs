using System.Threading;
using System.Threading.Tasks;

namespace Warzone.Authentication
{
    public interface IAuthenticationHandler
    {
        Task<bool> LoginAsync(string email, string password, CancellationToken? cancellationToken = null);
        bool LoggedIn { get; }
    }
}
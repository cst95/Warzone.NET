using System.Threading;
using System.Threading.Tasks;
using Warzone.Authentication;
using Warzone.Clients;
using Warzone.Http;

namespace Warzone
{
    public class WarzoneClient : IWarzoneClient
    {
        private readonly IAuthenticationHandler _authenticationHandler;
        private readonly ICodApiClient _codApiClient;

        public WarzoneClient()
        {
            var httpService = new HttpService();
            _codApiClient = new CodApiClient(httpService);
            _authenticationHandler = new AuthenticationHandler(_codApiClient);
        }

        public Task<bool> LoginAsync(string email, string password) =>
            _authenticationHandler.LoginAsync(email, password);

        public Task<object> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken = null) =>
            _codApiClient.GetLastTwentyWarzoneMatchesAsync(playerName, platform, cancellationToken);
    }
}
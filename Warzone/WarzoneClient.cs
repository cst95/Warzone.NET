using System.Threading.Tasks;
using Warzone.Authentication;
using Warzone.Http;

namespace Warzone
{
    public class WarzoneClient : IWarzoneClient
    {
        private readonly IHttpService _httpService;
        private readonly IAuthenticationHandler _authenticationHandler;
        
        public WarzoneClient()
        {
            _httpService = new HttpService();
            _authenticationHandler = new AuthenticationHandler(_httpService);
        }

        public Task<bool> LoginAsync(string email, string password) =>
            _authenticationHandler.LoginAsync(email, password);
    }
}
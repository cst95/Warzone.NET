using System.Threading.Tasks;
using Warzone.Authentication;
using Warzone.Http;

namespace Warzone
{
    public class WarzoneClient : IWarzoneClient
    {
        public const string BaseCookie =
            "new_SiteId=cod; ACT_SSO_LOCALE=en_US;country=US;XSRF-TOKEN=68e8b62e-1d9d-4ce1-b93f-cbe5ff31a041;API_CSRF_TOKEN=68e8b62e-1d9d-4ce1-b93f-cbe5ff31a041;";
        private readonly IHttpService _httpService;
        private readonly IAuthenticationHandler _authenticationHandler;
        
        public WarzoneClient()
        {
            _httpService = new HttpService(BaseCookie);
            _authenticationHandler = new AuthenticationHandler(_httpService, BaseCookie);
        }

        public Task<bool> LoginAsync(string email, string password) =>
            _authenticationHandler.LoginAsync(email, password);
    }
}
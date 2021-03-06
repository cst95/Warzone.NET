using System.Threading.Tasks;
using Warzone.Authentication;
using Warzone.Http;

namespace Warzone
{
    public class WarzoneClient : IWarzoneClient
    {
        private const string BaseCookie = "new_SiteId=cod; ACT_SSO_LOCALE=en_US;country=US;XSRF-TOKEN=68e8b62e-1d9d-4ce1-b93f-cbe5ff31a041;API_CSRF_TOKEN=68e8b62e-1d9d-4ce1-b93f-cbe5ff31a041;";
        private const string UserAgent = "a4b471be-4ad2-47e2-ba0e-e1f2aa04bff9";

        private readonly IAuthenticationHandler _authenticationHandler;
        
        public WarzoneClient()
        {
            var httpService = new HttpService(BaseCookie, UserAgent);
            _authenticationHandler = new AuthenticationHandler(httpService, BaseCookie);
        }

        public Task<bool> LoginAsync(string email, string password) =>
            _authenticationHandler.LoginAsync(email, password);
    }
}
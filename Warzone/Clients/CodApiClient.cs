using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Warzone.Authentication;
using Warzone.Constants;
using Warzone.Exceptions;
using Warzone.Http;

namespace Warzone.Clients
{
    public class CodApiClient : ICodApiClient
    {
        private readonly IAuthenticationHandler _authenticationHandler;
        private readonly IHttpService _httpService;
        private const string BaseUrl = "https://my.callofduty.com/api/papi-client/";

        public CodApiClient(IAuthenticationHandler authenticationHandler, IHttpService httpService)
        {
            _authenticationHandler = authenticationHandler;
            _httpService = httpService;
        }

        public async Task<object> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken)
        {
            if (!_authenticationHandler.LoggedIn)
                throw new NotLoggedInException();
            if (!Platforms.IsValid(platform))
                throw new ArgumentOutOfRangeException(nameof(platform));
            if (string.IsNullOrWhiteSpace(playerName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(playerName));
            if (string.IsNullOrWhiteSpace(platform))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(platform));

            var safePlayerName = HttpUtility.HtmlEncode(playerName);
            var url = $"{BaseUrl}crm/cod/{Versions.V2}/title/{Titles.Warzone}/platform/{platform}/gamer/{safePlayerName}/matches/wz/start/0/end/0/details";

            var response = await _httpService.GetAsync<object>(url, null, cancellationToken);

            return response;
        }
    }
}
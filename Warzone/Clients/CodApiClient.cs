using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Warzone.Constants;
using Warzone.Http;
using Warzone.Models.CodApi;

namespace Warzone.Clients
{
    public class CodApiClient : ICodApiClient
    {
        private readonly IHttpService _httpService;
        private const string BaseUrl = "https://my.callofduty.com/api/papi-client/";
        private const string LoginUrl = "https://profile.callofduty.com/do_login?new_SiteId=cod";
        private const string CsrfTokenUrl = "https://profile.callofduty.com/cod/login";

        public CodApiClient(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<string> FetchXsrfTokenAsync(CancellationToken? cancellationToken = null)
        {
            var initialResponse = await _httpService.GetAsync(CsrfTokenUrl, null, cancellationToken);

            return !initialResponse.Success ? null : ParseXsrfTokenFromHeaders(initialResponse.Headers);
        }

        public async Task<bool> LoginAsync(string email, string password, string xsrfToken,
            CancellationToken? cancellationToken = null)
        {
            var loginResponse = await _httpService.PostAsync(LoginUrl,
                GetLoginContent(email, password, xsrfToken),
                new Dictionary<string, string>() {{"Cookie", $"XSRF-TOKEN={xsrfToken}"}}, cancellationToken);

            if (!loginResponse.Success) return false;

            var (_, value) = loginResponse.Headers.First(f => f.Key == "Set-Cookie");

            _httpService.UpdateDefaultHeaders("Cookie", value);

            return true;
        }

        public async Task<CodApiResponse<Summaries>> GetLastTwentyWarzoneMatchesAsync(string playerName,
            string platform,
            CancellationToken? cancellationToken)
        {
            if (!Platforms.IsValid(platform))
                throw new ArgumentOutOfRangeException(nameof(platform));
            if (string.IsNullOrWhiteSpace(playerName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(playerName));
            if (string.IsNullOrWhiteSpace(platform))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(platform));

            var safePlayerName = HttpUtility.HtmlEncode(playerName);
            var url =
                $"{BaseUrl}crm/cod/{Versions.V2}/title/{Titles.Warzone}/platform/{platform}/gamer/{safePlayerName}/matches/wz/start/0/end/0/details";

            var response = await _httpService.GetAsync<SummariesWrapper>(url, null, cancellationToken);

            return new CodApiResponse<Summaries>
            {
                StatusCode = response.StatusCode,
                Success = response.Success,
                Error = response.Error,
                Data = response.Content?.Summary
            };
        }

        private static string ParseXsrfTokenFromHeaders(HttpResponseHeaders headers)
        {
            var xsrfHeader = headers.First(h => h.Key.Equals("Set-Cookie")).Value.First(v => v.Contains("XSRF-TOKEN"));
            var regex = new Regex("(XSRF-TOKEN=|;)");
            var cookieStrings = regex.Split(xsrfHeader.Trim());
            return cookieStrings[2];
        }

        private static FormUrlEncodedContent GetLoginContent(string email, string password, string token) =>
            new(new Dictionary<string, string>()
            {
                {"username", email},
                {"password", password},
                {"remember_me", "true"},
                {"_csrf", token},
            });
    }
}
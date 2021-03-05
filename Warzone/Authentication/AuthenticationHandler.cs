using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Http;

namespace Warzone.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private const string TokenUrl = "https://profile.callofduty.com/cod/login";
        private const string LoginUrl = "https://profile.callofduty.com/do_login?new_SiteId=co";
        private readonly IHttpService _httpService;
        private string _email;
        private string _password;
        
        public AuthenticationHandler(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public bool LoggedIn => !string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_password);
        
        public async Task<bool> LoginAsync(string email, string password, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

            var xsrfToken = await GetXsrfTokenAsync(cancellationToken);
            var loginDict = new Dictionary<string, string>
            {
                { "Cookie: ", $"XSRF_TOKEN={xsrfToken}"},
                { "username",  email},
                { "password", password},
                { "remember_me", "true" },
                { "_csrf", xsrfToken }
            };
            
            var content = new FormUrlEncodedContent(loginDict);

            var loginResponse = await _httpService.PostAsync<object>(LoginUrl, content, cancellationToken);

            return loginResponse.Success;
        }

        private async Task<string> GetXsrfTokenAsync(CancellationToken? cancellationToken)
        {
            var tokenResponse = await _httpService.GetAsync<string>(TokenUrl, cancellationToken);
            var (_, value) = tokenResponse.Headers.FirstOrDefault(h => h.Key == "Set-Cookie");
            var cookieValue = value.FirstOrDefault(v => v.Contains("XSRF-TOKEN"));
            return ParseCookie(cookieValue);
        }
        
        private string ParseCookie(string cookieValue)
        {
            var regex = new Regex("(XSRF-TOKEN=|;)");
            var cookieStrings = regex.Split(cookieValue.Trim());
            return cookieStrings[2];
        }
    }
}
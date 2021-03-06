using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Exceptions;
using Warzone.Http;
using Warzone.Models;

namespace Warzone.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private const string LoginUrl = "https://profile.callofduty.com/do_login?new_SiteId=cod";
        private const string CsrfTokenUrl = "https://profile.callofduty.com/cod/login";

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
            if (LoggedIn) throw new AlreadyLoggedInException();

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

            var initialResponse = await _httpService.GetAsync<object>(CsrfTokenUrl, null, cancellationToken);

            if (!initialResponse.Success) return false;

            var xsrfToken = GetXsrfToken(initialResponse.Headers);
            var loginResponse = await _httpService.PostAsync<object>(LoginUrl,
                GetLoginContent(email, password, xsrfToken),
                new Dictionary<string, string>() {{"Cookie", $"XSRF-TOKEN={xsrfToken}"}}, cancellationToken);

            if (!loginResponse.Success) return false;

            _email = email;
            _password = password;
            
            var (_, value) = loginResponse.Headers.First(f => f.Key == "Set-Cookie");
            
            _httpService.UpdateDefaultHeaders("Cookie", value);
            
            return true;
        }

        public Task LogoutAsync()
        {
            if (!LoggedIn) throw new NotLoggedInException();

            _email = null;
            _password = null;
            _httpService.ResetDefaultHeaders();

            return Task.CompletedTask;
        }

        private static FormUrlEncodedContent GetLoginContent(string email, string password, string token) =>
            new(new Dictionary<string, string>()
            {
                {"username", email},
                {"password", password},
                {"remember_me", "true"},
                {"_csrf", token},
            });

        private static string GetUniqId()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            var t = ts.TotalMilliseconds / 1000;

            var a = (int) Math.Floor(t);
            var b = (int) ((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }

        private static string GetXsrfToken(HttpResponseHeaders headers)
        {
            var xsrfHeader = headers.First(h => h.Key.Equals("Set-Cookie")).Value.First(v => v.Contains("XSRF-TOKEN"));
            var regex = new Regex("(XSRF-TOKEN=|;)");
            var cookieStrings = regex.Split(xsrfHeader.Trim());
            return cookieStrings[2];
        }
    }
}
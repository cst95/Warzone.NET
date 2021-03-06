using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Http;
using Warzone.Models;

namespace Warzone.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private const string BaseLoginUrl = "https://profile.callofduty.com/cod/mapp/";
        private readonly IHttpService _httpService;
        private readonly string _baseCookie;
        private string _email;
        private string _password;

        public AuthenticationHandler(IHttpService httpService, string baseCookie)
        {
            _httpService = httpService;
            _baseCookie = baseCookie;
        }

        public bool LoggedIn => !string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_password);

        public async Task<bool> LoginAsync(string email, string password, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

            var (deviceId, deviceIdJson) = GetDeviceId();
            var initialResponse = await _httpService.PostAsync<DeviceResponse>($"{BaseLoginUrl}registerDevice",
                new StringContent(deviceIdJson, Encoding.UTF8, MediaTypeNames.Application.Json), cancellationToken);

            if (!initialResponse.Content.InitialLoginSuccessful || !initialResponse.Success) return false;

            _httpService.UpdateDefaultHeaders("Authorization", $"bearer {initialResponse.Content.Data.AuthHeader}");
            _httpService.UpdateDefaultHeaders("x_cod_device_id", deviceId);

            var loginResponse =
                await _httpService.PostAsync<LoginResponse>($"{BaseLoginUrl}login", GetLoginContent(email, password),
                    cancellationToken);

            if (!(loginResponse.Success && loginResponse.Content.Success)) return false;

            _email = email;
            _password = password;

            var data = loginResponse.Content;
            var cookie = $"{_baseCookie}rtkn=${data.RToken};ACT_SSO_COOKIE=${data.SsoCookie};atkn=${data.AToken};";

            _httpService.UpdateDefaultHeaders("Cookie", cookie);

            return loginResponse.Success;
        }

        private static StringContent GetLoginContent(string email, string password) =>
            new(JsonSerializer.Serialize(new {email, password}), Encoding.UTF8,
                MediaTypeNames.Application.Json);

        private static (string, string) GetDeviceId()
        {
            var randomId = GetUniqId();

            using var md5 = MD5.Create();
            var deviceId = string.Join(string.Empty,
                md5.ComputeHash(Encoding.UTF8.GetBytes(randomId)).Select(b => b.ToString("x2")));

            return (deviceId, JsonSerializer.Serialize(new {deviceId}));
        }

        private static string GetUniqId()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            var t = ts.TotalMilliseconds / 1000;

            var a = (int) Math.Floor(t);
            var b = (int) ((t - Math.Floor(t)) * 1000000);

            return a.ToString("x8") + b.ToString("x5");
        }
    }
}
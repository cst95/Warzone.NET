﻿using System;
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
using Warzone.Clients;
using Warzone.Exceptions;
using Warzone.Http;
using Warzone.Models;

namespace Warzone.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly ICodApiClient _codApiClient;
        private string _email;
        private string _password;

        public AuthenticationHandler(ICodApiClient codApiClient)
        {
            _codApiClient = codApiClient;
        }

        public bool LoggedIn => !string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_password);

        public async Task<bool> LoginAsync(string email, string password, CancellationToken? cancellationToken = null)
        {
            if (LoggedIn) 
                throw new AlreadyLoggedInException();
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

            var xsrfToken = await _codApiClient.FetchXsrfTokenAsync(cancellationToken);
            var loginSuccess = await _codApiClient.LoginAsync(email, password, xsrfToken, cancellationToken);

            if (!loginSuccess) return false;

            _email = email;
            _password = password;

            return true;
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
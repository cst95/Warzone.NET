using System;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Clients;
using Warzone.Exceptions;

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
    }
}
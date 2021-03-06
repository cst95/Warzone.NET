using System.Text.Json.Serialization;

namespace Warzone.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        [JsonPropertyName("rtkn")] 
        public string RToken { get; set; }
        [JsonPropertyName("s_ACT_SSO_COOKIE")] 
        public string SsoCookie { get; set; }
        [JsonPropertyName("atkn")] 
        public string AToken { get; set; }
    }
}
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Warzone.Models.Http
{
    public class BaseHttpResponse
    {
        public HttpResponseHeaders Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public HttpContent ResponseContent { get; set; }
    }
}
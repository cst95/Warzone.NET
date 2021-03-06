using System.Net;
using System.Net.Http.Headers;

namespace Warzone.Models
{
    public class HttpResponse<T> where T: class
    {
        public bool Success { get; set; }
        public T Content { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
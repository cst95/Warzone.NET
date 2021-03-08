using Warzone.Models.CodApi;

namespace Warzone.Models.Http
{
    public class HttpResponse<T> : BaseHttpResponse where T : class
    {
        public HttpResponse() {}
        
        public HttpResponse(BaseHttpResponse baseClass)
        {
            Headers = baseClass.Headers;
            Success = baseClass.Success;
            ResponseContent = baseClass.ResponseContent;
            StatusCode = baseClass.StatusCode;
        }
        
        public T Content { get; set; }

        public Error Error { get; set; }
    }
}
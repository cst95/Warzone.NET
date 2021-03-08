using System.Net;

namespace Warzone.Models.CodApi
{
    public class CodApiResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public Error Error { get; set; }
        public bool IsError => Data == null && Error != null;
    }
}
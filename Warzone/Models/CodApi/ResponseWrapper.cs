namespace Warzone.Models.CodApi
{
    public class ResponseWrapper<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
    }
}
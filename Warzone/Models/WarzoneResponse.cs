namespace Warzone.Models
{
    public class WarzoneResponse<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
    }
}
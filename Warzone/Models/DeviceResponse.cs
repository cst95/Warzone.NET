namespace Warzone.Models
{
    public class DeviceResponse
    {
        public bool InitialLoginSuccessful => Status.Equals("success");
        public string Status { get; set; }
        public DeviceData Data { get; set; }
    }
}
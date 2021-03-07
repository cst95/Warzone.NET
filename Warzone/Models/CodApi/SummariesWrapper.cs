using System.Text.Json.Serialization;

namespace Warzone.Models.CodApi
{
    public class SummariesWrapper
    {
        [JsonPropertyName("summary")] public Summaries Summary { get; set; }
    }
}
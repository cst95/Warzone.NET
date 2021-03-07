using System.Text.Json.Serialization;

namespace Warzone.Models.CodApi
{
    public class Summaries
    {
        [JsonPropertyName("all")] public Summary All { get; set; }
        [JsonPropertyName("br_brsolo")] public Summary BrSolo { get; set; }
        [JsonPropertyName("br_brduos")] public Summary BrDuos { get; set; }
        [JsonPropertyName("br_brtrios")] public Summary BrTrios { get; set; }
        [JsonPropertyName("br_brquads")] public Summary BrQuads { get; set; }
    }
}
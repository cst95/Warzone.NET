using System.Text.Json.Serialization;

namespace Warzone.Models.CodApi
{
    public class Summary
    {
        [JsonPropertyName("matchesPlayed")] public double GamesPlayed { get; set; }
        [JsonPropertyName("kills")] public double Kills { get; set; }
        [JsonPropertyName("deaths")] public double Deaths { get; set; }
        [JsonPropertyName("assists")] public double Assists { get; set; }
        [JsonPropertyName("headshots")] public double Headshots { get; set; }
        [JsonPropertyName("score")] public double Score { get; set; }
        [JsonPropertyName("objectiveTeamWiped")] public double TeamWipes { get; set; }
        [JsonPropertyName("wallbangs")] public double Wallbangs { get; set; }
        [JsonPropertyName("avgLifeTime")] public double AverageLifetime { get; set; }
        [JsonPropertyName("killsPerGame")] public double KillsPerGame { get; set; }
        [JsonPropertyName("distanceTraveled")] public double DistanceTraveled { get; set; }
        [JsonPropertyName("kdRatio")] public double KdRatio { get; set; }
        [JsonPropertyName("scorePerGame")] public double ScorePerGame { get; set; }
        [JsonPropertyName("timePlayed")] public double TimePlayed { get; set; }
        [JsonPropertyName("gulagKills")] public double GulagKills { get; set; }
        [JsonPropertyName("gulagDeaths")] public double GulagDeaths { get; set; }
        [JsonPropertyName("damageDealt")] public double DamageDealt { get; set; }
        [JsonPropertyName("damageTaken")] public double DamageTaken { get; set; }
    }
}
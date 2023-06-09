﻿using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class PlayerGlobalRanking
    {
        [JsonPropertyName("PlayerToken")]
        public string PlayerToken { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Score")]
        public long Score { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class ResponseGlobalRanking
    {
        [JsonPropertyName("success")]
        public string success { get; set; }

        [JsonPropertyName("error_message")]
        public string error_message { get; set; }

        [JsonPropertyName("results")]
        public List<PlayerGlobalRanking> results { get; set; }

        [JsonPropertyName("metadata")]
        public object metadata { get; set; }
    }
}

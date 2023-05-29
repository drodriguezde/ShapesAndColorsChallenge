using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class ResponseUserProgress
    {
        [JsonPropertyName("success")]
        public string success { get; set; }

        [JsonPropertyName("error_message")]
        public string error_message { get; set; }

        [JsonPropertyName("results")]
        public List<UserData> results { get; set; }

        [JsonPropertyName("metadata")]
        public object metadata { get; set; }
    }
}

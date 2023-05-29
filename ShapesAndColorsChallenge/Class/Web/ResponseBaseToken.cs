using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class ResponseBaseToken
    {
        [JsonPropertyName("access_token")]
        public string access_token { get; set; }

        [JsonPropertyName("dtable_uuid")]
        public string dtable_uuid { get; set; }

        [JsonPropertyName("dtable_server")]
        public string dtable_server { get; set; }

        [JsonPropertyName("dtable_socket")]
        public string dtable_socket { get; set; }

        [JsonPropertyName("dtable_db")]
        public string dtable_db { get; set; }
    }
}

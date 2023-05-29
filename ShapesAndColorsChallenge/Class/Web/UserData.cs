using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class UserData
    {
        [JsonPropertyName("Data")]
        public string Data { get; set; }

        [JsonPropertyName("CRC")]
        public string CRC { get; set; }
    }
}

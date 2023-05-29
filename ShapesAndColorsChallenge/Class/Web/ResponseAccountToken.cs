using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class ResponseAccountToken
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
    }
}

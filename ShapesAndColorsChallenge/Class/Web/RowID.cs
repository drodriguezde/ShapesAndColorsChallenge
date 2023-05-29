using System.Text.Json.Serialization;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class RowID
    {
        [JsonPropertyName("_id")]
        public string _id { get; set; }
    }
}

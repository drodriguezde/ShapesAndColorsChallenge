using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

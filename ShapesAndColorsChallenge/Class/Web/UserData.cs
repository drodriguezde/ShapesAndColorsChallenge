using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

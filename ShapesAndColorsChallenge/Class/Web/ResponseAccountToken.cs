using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class ResponseAccountToken
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
    }
}

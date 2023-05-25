using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class BaseToken
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

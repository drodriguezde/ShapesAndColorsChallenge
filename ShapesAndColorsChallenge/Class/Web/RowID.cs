using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class RowID
    {
        [JsonPropertyName("_id")]
        public string _id { get; set; }
    }
}

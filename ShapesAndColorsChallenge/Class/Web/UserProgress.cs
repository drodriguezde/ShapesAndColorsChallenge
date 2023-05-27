using ShapesAndColorsChallenge.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Web
{
    public class UserProgress
    {
        [JsonPropertyName("Acheivement")]
        public List<Acheivement> Acheivements { get; set; }

        [JsonPropertyName("Challenge")]
        public List<Challenge> Challenges { get; set; }

        [JsonPropertyName("Perk")]
        public List<Perk> Perks { get; set; }

        [JsonPropertyName("Ranking")]
        public List<Ranking> Rankings { get; set; }

        [JsonPropertyName("Score")]
        public List<Score> Scores { get; set; }

        [JsonPropertyName("Settings")]
        public Settings Settings { get; set; }
    }
}

using ShapesAndColorsChallenge.Enum;
using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Score")]
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public GameMode GameMode { get; set; }

        [MaxLength(2)]
        public int StageNumber { get; set; }

        [MaxLength(2)]
        public int LevelNumber { get; set; }

        [MaxLength(8)]
        public long UserScore { get; set; }

        [MaxLength(1)]
        public int Stars { get; set; }
    }
}

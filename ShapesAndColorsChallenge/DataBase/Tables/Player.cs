using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Player")]
    internal class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public int PlayerID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(3)]
        public string Country { get; set; }

        public bool IsPlayer { get; set; }
    }
}

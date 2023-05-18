using ShapesAndColorsChallenge.Enum;
using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Perk")]
    internal class Perk
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public PerkType Type { get; set; }

        public int Amount { get; set; }
    }
}

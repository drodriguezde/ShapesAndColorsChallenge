using ShapesAndColorsChallenge.Enum;
using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Acheivement")]
    public class Acheivement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public AcheivementType Type { get; set; }

        /// <summary>
        /// Cantidad de veces que reclamo el logro.
        /// </summary>
        public int Claimed { get; set; }
    }
}

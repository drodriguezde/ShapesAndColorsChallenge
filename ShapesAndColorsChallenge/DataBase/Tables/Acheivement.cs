using ShapesAndColorsChallenge.Enum;
using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Acheivement")]
    internal class Acheivement
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

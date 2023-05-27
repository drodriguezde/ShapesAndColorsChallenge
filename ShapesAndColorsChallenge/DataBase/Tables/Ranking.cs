using ShapesAndColorsChallenge.Enum;
using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Ranking")]
    public class Ranking
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Clave ajena de la tabla Player.
        /// </summary>
        public int PlayerID { get; set; }

        public GameMode GameMode { get; set; }

        /// <summary>
        /// Cantidad de veces que ganó.
        /// </summary>
        public int Win { get; set; }

        /// <summary>
        /// Cantidad de veces que perdió.
        /// </summary>
        public int Lose { get; set; }
    }
}

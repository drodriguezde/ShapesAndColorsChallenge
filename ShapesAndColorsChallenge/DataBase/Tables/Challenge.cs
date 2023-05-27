using ShapesAndColorsChallenge.Enum;
using SQLite;
using System;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    [Table("Challenge")]
    public class Challenge
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// ID del retador, clave ajena de la tabla Player.
        /// </summary>
        public int PlayerID { get; set; }

        public ChallengeType ChallengeType  { get; set; }

        public GameMode GameMode { get; set; }

        [MaxLength(2)]
        public int StageNumber { get; set; }

        [MaxLength(2)]
        public int LevelNumber { get; set; }

        /// <summary>
        /// Fecha en que se lanzó el reto.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Indica si el reto está activo.
        /// </summary>
        public bool IsActive { get; set; }

        public bool Win { get; set; } = false;
    }
}

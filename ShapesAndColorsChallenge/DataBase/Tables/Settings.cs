using SQLite;

namespace ShapesAndColorsChallenge.DataBase.Tables
{
    internal class Settings
    {
        [PrimaryKey]
        public int Id { get; set; }

        [MaxLength(1)]
        public bool Notifications { get; set; }

        [MaxLength(1)]
        public bool Music { get; set; }

        [MaxLength(1)]
        public bool Sounds { get; set; }

        [MaxLength(1)]
        public bool Voices { get; set; }

        [MaxLength(1)]
        public bool Vibration { get; set; }

        [MaxLength(1)]
        public bool DarkMode { get; set; }

        [MaxLength(1)]
        public bool AlwaysDarkMode { get; set; }

        /// <summary>
        /// Idioma de la interfaz.
        /// </summary>
        [MaxLength(3)]
        public string CountryCode { get; set; }

        [MaxLength(20)]
        public string PlayerName { get; set; }

        /// <summary>
        /// Nacionalidad escogida por el jugador.
        /// </summary>
        [MaxLength(3)]
        public string PlayerCountryCode { get; set; }

        [MaxLength(16)]
        public string ShowHowToPlay { get; set; }
    }
}

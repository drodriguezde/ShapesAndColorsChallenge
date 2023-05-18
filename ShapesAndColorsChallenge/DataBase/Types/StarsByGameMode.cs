using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.DataBase.Types
{
    /// <summary>
    /// Este tipo contendrá la agrupación del número de estrellas por modo de juego.
    /// </summary>
    internal class StarsByGameMode
    {
        public GameMode GameMode { get; set; }
        public int Stars { get; set; }
    }
}

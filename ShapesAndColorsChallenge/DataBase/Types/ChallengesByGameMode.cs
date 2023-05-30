using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.DataBase.Types
{
    /// <summary>
    /// Este tipo contendrá la agrupación de las estrellas conseguidas en cada etapa de un modo de juego.
    /// </summary>
    internal class ChallengesByGameMode
    {
        public GameMode GameMode { get; set; }
        public int Counter { get; set; }
    }
}

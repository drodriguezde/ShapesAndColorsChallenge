namespace ShapesAndColorsChallenge.DataBase.Types
{
    /// <summary>
    /// Este tipo contendrá el ranking de jugadores por modo de juego.
    /// </summary>
    internal class RankingByGameMode
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public bool IsPlayer { get; set; }
        public long Points { get; set; }
        public int Win { get; set; }
        public int Position { get; set; }
    }
}

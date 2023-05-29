namespace ShapesAndColorsChallenge.Enum
{
    public enum AcheivementType : int
    {
        /// <summary>
        /// Cuando se consigue la primera estrella.
        /// </summary>
        FirstStar = 1,
        /// <summary>
        /// Cada 5 estrellas, hasta conseguir el máximo del juego, LEVELS * STAGES * MODES * 3
        /// </summary>
        StarCollector = 2,
        /// <summary>
        /// Todas en cada modo de juego.
        /// </summary>
        AllInClassic = 3,
        AllInEndless = 4,
        AllInMemory = 5,
        AllInTimeTrial = 6,
        AllInIncremental = 7,
        AllInMove = 8,
        AllInBlink = 9,
        AllInRotate = 10,
        AllInClassicPlus = 11,
        AllInEndlessPlus = 12,
        AllInMemoryPlus = 13,
        AllInTimeTrialPlus = 14,
        AllInIncrementalPlus = 15,
        AllInMovePlus = 16,
        AllInBlinkPlus = 17,
        AllInRotatePlus = 18,
        /// <summary>
        /// Todas en todos los modos normal.
        /// </summary>
        AllInNormal = 19,
        /// <summary>
        /// Todas en todos los modos plus.
        /// </summary>
        AllInPlus = 20,
        /// <summary>
        /// Todas en todos los modos.
        /// </summary>
        AllInAll = 21,
        /// <summary>
        /// Al conseguir la primera estrella en un modo cualquiera.
        /// </summary>
        FirstLevel = 22,
        /// <summary>
        /// Al conseguir una estrella en cada nivel de cada modo.
        /// </summary>
        FirstInClassic = 23,
        FirstInEndless = 24,
        FirstInMemory = 25,
        FirstInTimeTrial = 26,
        FirstInIncremental = 27,
        FirstInMove = 28,
        FirstInBlink = 29,
        FirstInRotate = 30,
        FirstInClassicPlus = 31,
        FirstInEndlessPlus = 32,
        FirstInMemoryPlus = 33,
        FirstInTimeTrialPlus = 34,
        FirstInIncrementalPlus = 35,
        FirstInMovePlus = 36,
        FirstInBlinkPlus = 37,
        FirstInRotatePlus = 38,
        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos nornal.
        /// </summary>
        FirstInNormal = 39,
        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos plus.
        /// </summary>
        FirstInPlus = 40,
        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos.
        /// </summary>
        FirstInAll = 41,
        /// <summary>
        /// Por superar el primer reto.
        /// </summary>
        FirstChallenge = 42,
        /// <summary>
        /// Por cada 5 retos superados, sin fin.
        /// </summary>
        Challenger = 43,
    }
}
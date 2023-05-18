using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Linq;

namespace ShapesAndColorsChallenge.Class
{
    internal static class GameData
    {
        #region CONST

        internal const int MODES = 16;
        internal const int MODES_NORMAL = 8;
        internal const int MODES_PLUS = 8;
        internal const int STAGES = 12;
        internal const int LEVELS = 12;
        internal const int STARS_PER_LEVEL = 3;

        /// <summary>
        /// Porcentaje necesario del total obtenible de puntos para alcanzar cada estrella.
        /// </summary>
        readonly static int[] PERCENT_TO_STAR = new int[3] { 25, 50, 80 };

        /// <summary>
        /// Tiempo en milisegundos disponible para cencontrar cada ficha por nivel, no por etapa.
        /// Según se va avanzando en los niveles se va reduciendo el tiempo.
        /// </summary>
        readonly static int[] TILE_TIME_PER_LEVEL = new int[12] { 4000, 3900, 3800, 3700, 3600, 3500, 3400, 3300, 3200, 3100, 3000, 2900 };

        /// <summary>
        /// Cantidad de fichas a encontrar por etapa.
        /// </summary>
        readonly static int[] TILES_PER_STAGE = new int[12] { 20, 22, 24, 26, 28, 30, 32, 34, 36, 40, 42, 44 };

        /// <summary>
        /// Número de fichas en vertical que tendrá cada nivel.
        /// </summary>
        readonly static int[,] VERTICAL_TILES_NUMBER = new int[/*Etapa*/,/*Nivel*/]  {
            {2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8},
            {3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8},
            {3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8},
            {4, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 8},
            {4, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 8},
            {5, 5, 5, 5, 5, 5, 5, 6, 6, 7, 7, 8},
            {5, 5, 5, 5, 5, 5, 5, 6, 6, 7, 7, 8},
            {6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 8},
            {6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 8},
            {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8},
            {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8},
            {8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8}
        };

        /// <summary>
        /// Número de fichas en horizontal que tendrá cada nivel.
        /// </summary>
        readonly static int[,] HORIZONTAL_TILES_NUMBER = new int[/*Etapa*/,/*Nivel*/] {
            {2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7},
            {2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7},
            {3, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7},
            {3, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7},
            {4, 4, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7},
            {4, 4, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7},
            {5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 7, 7},
            {5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 7, 7},
            {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7},
            {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7},
            {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7},
            {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7}
        };

        /// <summary>
        /// Cantidad de colores posibles por nivel.
        /// </summary>
        readonly static int[,] COLORS_NUMBER = new int[/*Etapa*/,/*Nivel*/] {
            {3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {5, 5, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {6, 6, 6, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {7, 7, 7, 7, 7, 8, 9, 10, 11, 12, 13, 14},
            {8, 8, 8, 8, 8, 8, 9, 10, 11, 12, 13, 14},
            {9, 9, 9, 9, 9, 9, 9, 10, 11, 12, 13, 14},
            {10, 10, 10, 10, 10, 10, 10, 10, 11, 12, 13, 14},
            {11, 11, 11, 11, 11, 11, 11, 11, 11, 12, 13, 14},
            {12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 13, 14},
            {13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 14},
            {14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14},
        };

        /// <summary>
        /// Cantidad de formas posibles por nivel.
        /// </summary>
        readonly static int[,] SHAPES_NUMBER = new int[/*Etapa*/,/*Nivel*/] {
            {3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {5, 5, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {6, 6, 6, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            {7, 7, 7, 7, 7, 8, 9, 10, 11, 12, 13, 14},
            {8, 8, 8, 8, 8, 8, 9, 10, 11, 12, 13, 14},
            {9, 9, 9, 9, 9, 9, 9, 10, 11, 12, 13, 14},
            {10, 10, 10, 10, 10, 10, 10, 10, 11, 12, 13, 14},
            {11, 11, 11, 11, 11, 11, 11, 11, 11, 12, 13, 14},
            {12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 13, 14},
            {13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 14},
            {14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14},
        };

        #endregion

        #region PROPERTIES

        internal static int StarsToUnlockMode
        {
            get 
            {
                return STAGES * LEVELS * STARS_PER_LEVEL / 3 * 2;
            }
        }

        /// <summary>
        /// Cantidad de fichas a entrontrar para la etapa actual.
        /// </summary>
        internal static int TilesCurrenStage
        {
            get
            {
                return TILES_PER_STAGE[OrchestratorManager.StageNumber - 1];
            }
        }

        /// <summary>
        /// Deveulve la puntuación de cada ficha encontrada en un modo contrareloj.
        /// </summary>
        internal static int GetTimeTrialPointsForFindedTile
        {
            get
            {
                return TILE_TIME_PER_LEVEL[OrchestratorManager.LevelNumber - 1];
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Devuelve la cantidad total de puntos máxima obtenible en modos no infinitos.
        /// El total de fichas del nivel por el total de tiempo por ficha.
        /// </summary>
        internal static int MaxPointStageLevel(int level)
        {
            return TilesCurrenStage * TimeCurrentLevel(level);
        }

        /// <summary>
        /// Obtiene el tiempo máximo disponible en el modo contrareloj par aencontrar todas las fichas que sea posible.
        /// </summary>
        internal static int TimeTrialModeTime(int level)
        {
            return TilesCurrenStage * TimeCurrentLevel(level);
        }

        /// <summary>
        /// Tiempo disponible para cada ficha en el nivel actual.
        /// </summary>
        internal static int TimeCurrentLevel(int level)
        {
            return TILE_TIME_PER_LEVEL[level - 1];
        }

        /// <summary>
        /// Devuelve el número de colores de ficha correspondientes a la etapa y nivel actual.
        /// </summary>
        internal static int ColorsNumber(int stage, int level)
        {
            return COLORS_NUMBER[stage - 1, level - 1];
        }

        /// <summary>
        /// Devuelve un color aleatorio de los posibles para el juego actual.
        /// Si se está en modo oscuro y sale negro se cambia por blanco.
        /// </summary>
        internal static TileColor RandomColor(int stage, int level)
        {
            TileColor color = (TileColor)Statics.GetRandom(1, ColorsNumber(stage, level));

            if (color == TileColor.Black && Statics.IsDarkModeActive)
                return TileColor.White;
            else
                return color;
        }

        /// <summary>
        /// Devuelve el número de tipos de ficha correspondientes a la etapa y nivel actual.
        /// </summary>
        internal static int ShapesNumber(int stage, int level)
        {
            return SHAPES_NUMBER[stage - 1, level - 1];
        }

        /// <summary>
        /// Devuelve una forma aleatoria de las posibles para el juego actual.
        /// </summary>
        internal static ShapeType RandomShape(int stage, int level)
        {
            return (ShapeType)Statics.GetRandom(1, ShapesNumber(stage, level));
        }

        /// <summary>
        /// Devuelve el tamaño de  grid en base a número de fichas vertical.
        /// </summary>
        internal static int VerticalTilesNumber(int stage, int level)
        {
            return VERTICAL_TILES_NUMBER[stage - 1, level - 1];
        }

        /// <summary>
        /// Devuelve el tamaño de  grid en base a número de fichas horizontal.
        /// </summary>
        internal static int HorizontalTilesNumber(int stage, int level)
        {
            return HORIZONTAL_TILES_NUMBER[stage - 1, level - 1];
        }

        /// <summary>
        /// Devuelve el número de estrellas que ha conseguido un usuario con una puntuación.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        internal static int GetStarsForThisPoints(long points, int level)
        {
            if (points >= PointsToStar(2, level))
                return 3;
            else if (points >= PointsToStar(1, level))
                return 2;
            else if (points >= PointsToStar(0, level))
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Cantidad de puntos necesaria para alcanzar las estrellas.
        /// </summary>
        internal static int PointsToStar(int starIndex, int level)
        {
            return (MaxPointStageLevel(level) / 100f * PERCENT_TO_STAR[starIndex]).ToInt();
        }

        /// <summary>
        /// Porcentaje necesario para alcanzar las estrellas.
        /// </summary>
        internal static int PercentToStar(int starIndex)
        {
            return PERCENT_TO_STAR[starIndex];
        }

        /// <summary>
        /// Devuelve la cantidad de estrellas necesarias para desbloquear un nivel.
        /// </summary>
        /// <returns></returns>
        internal static int StarsToUnlockLevel(int level)
        {
            return (int)Math.Floor((level - 1) * STARS_PER_LEVEL / 2d);
        }

        /// <summary>
        /// Devuelve la cantidad de estrallas necesarios para desbloquear una etapa.
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        internal static int StarsToUnlockStage(int stage)
        {
            return 5 * (int)Math.Floor((stage - 1) * STARS_PER_LEVEL * LEVELS / 10d);
        }

        #endregion
    }
}

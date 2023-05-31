using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerScore
    {
        #region METHODS

        /// <summary>
        /// Despliega la tabla de puntuaciones de usuario en caso de no existir.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Score>();
            DataBaseManager.Connection.CreateTable<Score>();

            if (Any())
                return;

            DataBaseManager.Connection.BeginTransaction();

            foreach (GameMode gameMode in System.Enum.GetValues(typeof(GameMode)))
                if (gameMode != GameMode.None)
                    for (int stageNumber = 1; stageNumber <= 12; stageNumber++)
                        for (int levelNumber = 1; levelNumber <= 12; levelNumber++)
                        {
                            Score newUserScore = new()
                            {
                                GameMode = gameMode,
                                StageNumber = stageNumber,
                                LevelNumber = levelNumber,
                                UserScore = 0,
                                Stars = 0,
                                TilesFinded = 0,
                            };
                            DataBaseManager.Connection.Insert(newUserScore);
                        }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Establece la tabla en base a los datos descargados desde una partida guardada en la nube.
        /// </summary>
        internal static void Restore(List<Score> scores)
        {
            DataBaseManager.Connection.DropTable<Score>();
            DataBaseManager.Connection.CreateTable<Score>();
            DataBaseManager.Connection.BeginTransaction();

            foreach (Score score in scores)
            {
                Score newUserScore = new()
                {
                    GameMode = score.GameMode,
                    StageNumber = score.StageNumber,
                    LevelNumber = score.LevelNumber,
                    UserScore = score.UserScore,
                    Stars = score.Stars,
                    TilesFinded = score.TilesFinded,
                };
                DataBaseManager.Connection.Insert(newUserScore);
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Obtiene un listado con todas las puntuaciones del usuario.
        /// </summary>
        /// <returns></returns>
        internal static List<Score> Get()
        {
            return DataBaseManager.Connection.Table<Score>().ToList();
        }

        /// <summary>
        /// Obtiene un listado con todas las puntuaciones del usuario.
        /// </summary>
        /// <returns></returns>
        internal static List<Score> Get(GameMode gameMode)
        {
            return DataBaseManager.Connection.Table<Score>().Where(t => t.GameMode == gameMode).ToList();
        }

        /// <summary>
        /// Obtiene un listado con todas las puntuaciones del usuario por modo de juego.
        /// </summary>
        /// <returns></returns>
        internal static List<Score> Get(GameMode gameMode, int stageNumber)
        {
            return DataBaseManager.Connection.Table<Score>().Where(t => t.GameMode == gameMode && t.StageNumber == stageNumber).OrderBy(t => t.LevelNumber).ToList();
        }

        /// <summary>
        /// Obtiene un score de un nivel en particular.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="stageNumber"></param>
        /// <param name="levelNumber"></param>
        /// <returns></returns>
        internal static Score Get(GameMode gameMode, int stageNumber, int levelNumber)
        {
            return DataBaseManager.Connection.Table<Score>().Where(t => t.GameMode == gameMode && t.StageNumber == stageNumber && t.LevelNumber == levelNumber).First();
        }

        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Score>().Any();
        }

        /// <summary>
        /// Actualiza una puntuación.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        internal static bool Update(Score score)
        {
            return DataBaseManager.Connection.Update(score) > 0;
        }

        internal static void Reset()
        {
            List<Score> userScores = Get();

            foreach (Score userScore in userScores)
            {
                userScore.UserScore = 0;
                userScore.Stars = 0;
                userScore.TilesFinded = 0;
            }

            DataBaseManager.Connection.UpdateAll(userScores, true);
        }

        #endregion
    }
}

using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerChallenge
    {
        #region CONST



        #endregion

        #region METHODS

        /// <summary>
        /// Despliega la tabla de retos en caso de no existir.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Challenge>();
            DataBaseManager.Connection.CreateTable<Challenge>();
        }

        /// <summary>
        /// Establece la tabla en base a los datos descargados desde una partida guardada en la nube.
        /// </summary>
        /// <param name="challenges"></param>
        internal static void Deploy(List<Challenge> challenges)
        {
            DataBaseManager.Connection.DropTable<Challenge>();
            DataBaseManager.Connection.CreateTable<Challenge>();
            DataBaseManager.Connection.BeginTransaction();

            foreach (Challenge challenge in challenges)
            {
                Challenge newChallenge = new()
                {
                    PlayerID = challenge.PlayerID,
                    ChallengeType = challenge.ChallengeType,
                    GameMode = challenge.GameMode,
                    StageNumber = challenge.StageNumber,
                    LevelNumber = challenge.LevelNumber,
                    StartDate = challenge.StartDate,
                    IsActive = challenge.IsActive,
                    Win = challenge.Win,
                };
                DataBaseManager.Connection.Insert(newChallenge);
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Vacia el contenido de la tabla.
        /// </summary>
        internal static void Reset()
        {
            DataBaseManager.Connection.DropTable<Challenge>();
            Deploy();
        }

        /// <summary>
        /// Obtiene un listado con todos los retos.
        /// </summary>
        /// <returns></returns>
        internal static List<Challenge> Get()
        {
            return DataBaseManager.Connection.Table<Challenge>().ToList();
        }

        /// <summary>
        /// Comprueba si existe algún registro en la tabla.
        /// </summary>
        /// <returns></returns>
        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Challenge>().Any();
        }

        /// <summary>
        /// Actualiza un registro de reto.
        /// </summary>
        /// <param name="challenge"></param>
        /// <returns></returns>
        internal static bool Update(Challenge challenge)
        {
            return DataBaseManager.Connection.Update(challenge) > 0;
        }

        #endregion
    }
}

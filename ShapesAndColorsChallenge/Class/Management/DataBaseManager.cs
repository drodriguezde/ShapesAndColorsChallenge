/***********************************************************************
* DESCRIPTION :
*
*
* NOTES :
* 
* 
* WARNINGS :
* 
* 
* OPTIMIZE IMPORTS : NO
* EXCEPTION CONTROL : NO
* DISPOSE CONTROL : YES
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class DataBaseManager
    {
        #region VARS



        #endregion

        #region PROPERTIES

        internal static SQLiteConnection Connection { get; private set; }

        #endregion

        #region METHODS

        internal static void Initialize()
        {
            Connection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ShapesAndColorsChallenge.db3"));
            ControllerScore.Deploy();
            ControllerSettings.Deploy();
            ControllerRanking.Deploy();
            ControllerPerk.Deploy();
            ControllerAcheivement.Deploy();
        }

        /// <summary>
        /// Reinicia el progreso del usuario.
        /// </summary>
        internal static void ResetProgress()
        {
            /*TODO, tambien hay que reiniciar los retos*/
            ControllerScore.Reset();
            ControllerRanking.Reset();
        }

        /// <summary>
        /// Obtiene un listado de todas las estrellas que tiene el usuario por modo de juego.
        /// </summary>
        /// <returns></returns>
        internal static List<StarsByGameMode> GetUserStarsGroupByStage()
        {
            return (from userScore in ControllerScore.Get()
                    group userScore.Stars by new { userScore.GameMode } into g
                    select new StarsByGameMode { GameMode = g.Key.GameMode, Stars = g.Sum() }).ToList();
        }

        /// <summary>
        /// Obtiene un listado de todas las estrellas que tiene el usuario en cada etapa de un modo de juego determinado.
        /// </summary>
        /// <returns></returns>
        internal static List<StarsByStage> GetUserStarsGroupByStage(GameMode gameMode)
        {
            return (from userScore in ControllerScore.Get(gameMode)
                    group userScore.Stars by new { userScore.GameMode, userScore.StageNumber } into g
                    select new StarsByStage { Stage = g.Key.StageNumber, Stars = g.Sum() }).ToList();
        }

        #endregion
    }
}

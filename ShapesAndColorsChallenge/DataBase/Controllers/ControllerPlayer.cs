using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerPlayer
    {
        #region CONST



        #endregion

        #region METHODS

        /// <summary>
        /// Obtiene un listado con todos los jugadores.
        /// </summary>
        /// <returns></returns>
        internal static List<Player> Get()
        {
            return DataBaseManager.Connection.Table<Player>().ToList();
        }

        /// <summary>
        /// Comprueba si existe algún registro en la tabla.
        /// </summary>
        /// <returns></returns>
        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Player>().Any();
        }

        /// <summary>
        /// Actualiza un registro de jugador.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        internal static bool Update(Player player)
        {
            return DataBaseManager.Connection.Update(player) > 0;
        }

        #endregion
    }
}

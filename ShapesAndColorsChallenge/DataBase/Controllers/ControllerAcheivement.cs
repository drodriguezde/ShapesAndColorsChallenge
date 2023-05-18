using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerAcheivement
    {
        #region METHODS

        /// <summary>
        /// Despliega la tabla de logros en caso de no existir.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Acheivement>();
            DataBaseManager.Connection.CreateTable<Acheivement>();

            if (Any())
                return;

            DataBaseManager.Connection.BeginTransaction();

            foreach (AcheivementType acheivementType in System.Enum.GetValues(typeof(AcheivementType)))
            {
                Acheivement acheivement = new()
                {
                    Type = acheivementType,
                    Claimed = 0
                };
                DataBaseManager.Connection.Insert(acheivement);
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Obtiene un listado con todos los logros.
        /// </summary>
        /// <returns></returns>
        internal static List<Acheivement> Get()
        {
            return DataBaseManager.Connection.Table<Acheivement>().ToList();
        }

        /// <summary>
        /// Obtiene un logro determinado.
        /// </summary>
        /// <returns></returns>
        internal static Acheivement Get(AcheivementType type)
        {
            return DataBaseManager.Connection.Table<Acheivement>().Single(t => t.Type == type);
        }

        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Acheivement>().Any();
        }

        /// <summary>
        /// Actualiza un logro.
        /// </summary>
        /// <param name="acheivement"></param>
        /// <returns></returns>
        internal static bool Update(Acheivement acheivement)
        { 
            return DataBaseManager.Connection.Update(acheivement) > 0;
        }

        #endregion
    }
}

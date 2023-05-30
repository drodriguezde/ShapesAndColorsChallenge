using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerPerk
    {
        #region METHODS

        /// <summary>
        /// Genera la tabla de Ranking en caso de no existir.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Perk>();
            DataBaseManager.Connection.CreateTable<Perk>();

            if (Any())
                return;

            DataBaseManager.Connection.BeginTransaction();/*Usamos una transacción para insertar todos los registros de golpe en bulk*/

            foreach (PerkType perkType in System.Enum.GetValues(typeof(PerkType)))
            {
                Perk perk = new()
                {
                    Type = perkType,
                    Amount = 2/*El jugador inicia con 2 potenciadores de cada tipo*/
                };
                DataBaseManager.Connection.Insert(perk);
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Establece la tabla en base a los datos descargados desde una partida guardada en la nube.
        /// </summary>
        internal static void Restore(List<Perk> perks)
        {
            DataBaseManager.Connection.DropTable<Perk>();
            DataBaseManager.Connection.CreateTable<Perk>();
            DataBaseManager.Connection.BeginTransaction();/*Usamos una transacción para insertar todos los registros de golpe en bulk*/

            foreach (Perk perk in perks)
            {
                Perk newPerk = new()
                {
                    Type = perk.Type,
                    Amount = perk.Amount
                };
                DataBaseManager.Connection.Insert(newPerk);
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Obtiene todos los potenciadores.
        /// </summary>
        /// <returns></returns>
        internal static List<Perk> Get()
        {
            return DataBaseManager.Connection.Table<Perk>().ToList();
        }

        /// <summary>
        /// Obtiene un tipo de potenciador en concreto.
        /// </summary>
        /// <returns></returns>
        internal static Perk Get(PerkType perkType)
        {
            return DataBaseManager.Connection.Table<Perk>().Single(t => t.Type == perkType);
        }

        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Perk>().Any();
        }

        internal static void Discount(PerkType perkType)
        {
            Perk perk = Get(perkType);

            if (perk.Amount == 0)
                return;

            perk.Amount--;
            DataBaseManager.Connection.Update(perk);
        }

        /// <summary>
        /// Actualiza un potenciador.
        /// </summary>
        /// <param name="perk"></param>
        /// <returns></returns>
        internal static bool Update(Perk perk)
        {
            return DataBaseManager.Connection.Update(perk) > 0;
        }

        #endregion
    }
}

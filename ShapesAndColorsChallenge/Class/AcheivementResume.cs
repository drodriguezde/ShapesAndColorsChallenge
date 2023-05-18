using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class
{
    internal class AcheivementResume
    {
        #region PROPERTIES

        /// <summary>
        /// Cantidad pendiente de este logro.
        /// Los que tiene según su progresos menos los ya reclamados.
        /// </summary>
        internal int Pending
        {
            get
            {
                return Max == Claimed ? 0 : Acheived - Claimed;
            }
        }

        /// <summary>
        /// Cuantos ha obtenido de este tipo.
        /// </summary>
        internal int Claimed { get; private set; } = 0;

        /// <summary>
        /// Tipo de logro.
        /// </summary>
        internal AcheivementType AcheivementType { get; private set; }

        /// <summary>
        /// Cantidad máxima que se puede obtener de este tipo.
        /// </summary>
        internal int Max { get; set; } = 0;

        /// <summary>
        /// Cantidad lograda, la calculada según su progreso.
        /// </summary>
        internal int Acheived { get; set; } = 0;

        /// <summary>
        /// Tipo de recompensa por el logro.
        /// </summary>
        internal RewardType RewardType{ get; set; } = RewardType.None;

        /// <summary>
        /// Indica si un logro está completa, es decir, reclamado en su totalidad.
        /// </summary>
        internal bool Completed 
        {
            get { return Max == Claimed; }
        }

        #endregion

        #region CONSTRUCTORS

        internal AcheivementResume(Acheivement acheivement, RewardType rewardType)
        {
            Claimed = acheivement.Claimed;
            AcheivementType = acheivement.Type;
            RewardType = rewardType;
        }

        #endregion
    }
}

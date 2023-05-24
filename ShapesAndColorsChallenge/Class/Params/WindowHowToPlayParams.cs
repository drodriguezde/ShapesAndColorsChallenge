using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class WindowHowToPlayParams
    {
        #region PROPERTIES

        /// <summary>
        /// Modo de juego del que se debe mostrar la información.
        /// </summary>
        internal GameMode GameMode { get; private set; }

        /// <summary>
        /// Indica si el origen es el botón de información que hay en la pantalla de selección de etapa de un modo de juego concreto.
        /// Si es así quiere decir que hay que poner el botón volver y no hay que mostrar el checkbox "No mostrar de nuevo".
        /// </summary>
        internal bool OriginInfoButton { get; private set; }

        #endregion

        #region CONSTRUCTOR

        internal WindowHowToPlayParams(GameMode gameMode, bool originInfoButton)
        {
            GameMode = gameMode;
            OriginInfoButton = originInfoButton;
        }

        #endregion
    }
}

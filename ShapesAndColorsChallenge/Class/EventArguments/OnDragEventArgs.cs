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
* DISPOSE CONTROL : NONULLABLE
*
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Microsoft.Xna.Framework;
using System;

namespace ShapesAndColorsChallenge.Class.EventArguments
{
    internal class OnDragEventArgs : EventArgs
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        internal Vector2 Start { get; set; }

        internal Vector2 Current { get; set; }

        /// <summary>
        /// Devuelve la coordenada en pantalla.
        /// </summary>
        internal Vector2 StartScreen
        {
            get
            {
                return Start;
            }
        }

        /// <summary>
        /// Devuelve la coordenada en pantalla.
        /// </summary>
        internal Vector2 CurrentScreen
        {
            get
            {
                return Current;
            }
        }

        #endregion

        #region CONSTRUCTORS

        internal OnDragEventArgs(Vector2 start, Vector2 current)
        {
            Start = start;
            Current = current;
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS



        #endregion
    }
}
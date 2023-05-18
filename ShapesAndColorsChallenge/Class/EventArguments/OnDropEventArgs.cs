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
    internal class OnDropEventArgs : EventArgs
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

        internal object Tag { get; private set; }

        internal Vector2 Position { get; private set; }

        #endregion

        #region CONSTRUCTORS

        internal OnDropEventArgs(Vector2 position)
        {
            Position = position;
        }

        internal OnDropEventArgs(object tag)
        {
            Tag = tag;
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS



        #endregion
    }
}
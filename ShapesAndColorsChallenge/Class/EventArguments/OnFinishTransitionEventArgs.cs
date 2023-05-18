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

using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.EventArguments
{
    internal class OnFinishTransitionEventArgs : EventArgs
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

        internal TransitionType TransitionType { get; private set; }

        #endregion

        #region CONSTRUCTORS

        internal OnFinishTransitionEventArgs(TransitionType transitionType)
        {
            TransitionType = transitionType;
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS



        #endregion
    }
}
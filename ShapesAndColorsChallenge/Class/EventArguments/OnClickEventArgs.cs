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

using InputHelper;
using System;

namespace ShapesAndColorsChallenge.Class.EventArguments
{
    internal class OnClickEventArgs : EventArgs
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

        internal ClickEventArgs ClickEventArgs { get; private set; }

        #endregion

        #region CONSTRUCTORS

        internal OnClickEventArgs(ClickEventArgs clickEventArgs)
        {
            ClickEventArgs = clickEventArgs;
        }

        internal OnClickEventArgs(object tag)
        {
            Tag = tag;
        }

        internal OnClickEventArgs(ClickEventArgs clickEventArgs, object tag)
        {
            ClickEventArgs = clickEventArgs;
            Tag = tag;
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS



        #endregion
    }
}
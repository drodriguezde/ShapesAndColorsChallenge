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

using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using ShapesAndColorsChallenge.Class.EventArguments;
using System;
using TouchScreenBuddy;

namespace ShapesAndColorsChallenge.Class.Management
{
    /// <summary>
    /// Gestiona los gestos y toques en la pantalla.
    /// </summary>
    internal static class TouchManager
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES

        internal static event EventHandler<OnClickEventArgs> OnClick;
        internal static event EventHandler<DragEventArgs> OnDrag;
        internal static event EventHandler<DropEventArgs> OnDrop;

        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        internal static TouchComponent TouchComponent { get; private set; } = null;

        #endregion

        #region CONSTRUCTORS



        #endregion

        #region DESTRUCTOR



        #endregion

        #region METHODS

        internal static void Initialize(Game game)
        {
            TouchComponent = new TouchComponent(game, null)
            {
                SupportedGestures = GestureType.Tap | GestureType.Pinch | GestureType.PinchComplete | GestureType.DoubleTap | GestureType.Flick
            };
        }

        internal static void Update()
        {
            if (TouchComponent.Clicks.Count > 0)
                OnClick?.Invoke(TouchComponent, new OnClickEventArgs(TouchComponent.Clicks[0]));

            if (TouchComponent.Drags.Count > 0)
                OnDrag?.Invoke(TouchComponent, TouchComponent.Drags[0]);

            if (TouchComponent.Drops.Count > 0)
                OnDrop?.Invoke(TouchComponent, TouchComponent.Drops[0]);
        }

        #endregion
    }
}
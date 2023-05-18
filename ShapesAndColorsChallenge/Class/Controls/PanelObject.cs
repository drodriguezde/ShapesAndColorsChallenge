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

using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class PanelObject
    {
        #region PROPERTIES

        /// <summary>
        /// Índice del panel.
        /// </summary>
        internal int PanelIndex { get; set; } = 0;

        /// <summary>
        /// Objecto agregado al panel.
        /// </summary>
        internal object AddedObject { get; private set; }

        /// <summary>
        /// Tipo del objeto añadido.
        /// </summary>
        internal Type Type { get; private set; }

        internal float X1 { get; set; }
        internal float X2 { get; set; }
        internal float Y1 { get; set; }
        internal float Y2 { get; set; }
        internal float X1Original { get; private set; }
        internal float X2Original { get; private set; }
        internal float Y1Original { get; private set; }
        internal float Y2Original { get; private set; }

        #endregion

        #region CONSTRUCTORS

        internal PanelObject(int panelIndex, object addedObject)
        {
            PanelIndex = panelIndex;
            AddedObject = addedObject;
            Type = addedObject.GetType();
            SetBounds();
        }

        #endregion

        #region METHODS

        void SetBounds()
        {
            if (Type.Name == typeof(Button).Name)
            {
                X1 = X1Original = (AddedObject as Button).Location.X;
                Y1 = Y1Original = (AddedObject as Button).Location.Y;
            }
            else if (Type.Name == typeof(Label).Name)
            {
                X1 = X1Original = (AddedObject as Label).Location.X;
                Y1 = Y1Original = (AddedObject as Label).Location.Y;
            }
            else if (Type.Name == typeof(Image).Name)
            {
                X1 = X1Original = (AddedObject as Image).Location.X;
                Y1 = Y1Original = (AddedObject as Image).Location.Y;
            }
            else if (Type.Name == typeof(RectangleSquare).Name)
            {
                X1 = X1Original = (AddedObject as RectangleSquare).Location.X;
                Y1 = Y1Original = (AddedObject as RectangleSquare).Location.Y;
            }
            else if (Type.Name == typeof(Line).Name)
            {
                X1 = X1Original = (AddedObject as Line).P1X;
                X2 = X2Original = (AddedObject as Line).P2X;
                Y1 = Y1Original = (AddedObject as Line).P1Y;
                Y2 = Y2Original = (AddedObject as Line).P2Y;
            }
        }

        #endregion
    }
}

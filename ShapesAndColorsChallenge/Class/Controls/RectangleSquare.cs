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
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class RectangleSquare : InteractiveObject
    {
        #region PROPERTIES

        internal int Thickness { get; set; } = 1;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds">Tamaño del rectangulo, debe pasarse redimensionado.</param>
        /// <param name="color"></param>
        /// <param name="thickness">Grosor del borde, debe pasarse redimensionado.</param>
        /// <param name="visible"></param>
        internal RectangleSquare(ModalLevel modalLevel, Rectangle bounds, Color color, int thickness, bool visible = true)
            : base(modalLevel, bounds)
        {
            ColorLightMode = color;
            ColorDarkMode = color;
            Thickness = thickness;
            Visible = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds">Tamaño del rectangulo, debe pasarse redimensionado.</param>
        /// <param name="colorLightMode"></param>
        /// <param name="colorDarkMode"></param>
        /// <param name="thickness">Grosor del borde, debe pasarse redimensionado.</param>
        /// <param name="visible"></param>
        internal RectangleSquare(ModalLevel modalLevel, Rectangle bounds, Color colorLightMode, Color colorDarkMode, int thickness, bool visible = true)
            : base(modalLevel, bounds)
        {
            ColorLightMode = colorLightMode;
            ColorDarkMode = colorDarkMode;
            Thickness = thickness;
            Visible = visible;
        }

        #endregion

        #region METHODS

        internal override void Draw(GameTime gameTime)
        {
            if (Visible)
                Screen.SpriteBatch.DrawRectangle(new Rectangle(Location.X.ToInt(), Location.Y.ToInt(), Bounds.Width, Bounds.Height), ColorMode, CurrentTransparency, Thickness);
        }

        #endregion
    }
}
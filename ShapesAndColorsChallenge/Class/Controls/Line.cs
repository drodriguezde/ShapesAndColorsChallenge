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
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Line : InteractiveObject
    {
        #region PROPERTIES

        internal int Thickness { get; set; } = 1;

        internal int P1X { get; set; } = 0;
        internal int P1Y { get; set; } = 0;
        internal int P2X { get; set; } = 0;
        internal int P2Y { get; set; } = 0;

        internal int OriginalP1X { get; private set; }
        internal int OriginalP1Y { get; private set; }
        internal int OriginalP2X { get; private set; }
        internal int OriginalP2Y { get; private set; }

        internal Point Point1 { get { return new Point(P1X, P1Y); } }

        internal Point Point2 { get { return new Point(P2X, P2Y); } }

        internal int Distance
        {
            get
            {
                return Math.Sqrt(Math.Pow(P1X - P2X, 2) + Math.Pow(P1Y - P2Y, 2)).ToInt();
            }
        }

        internal float Angle
        {
            get
            {
                return Math.Atan2(P2Y - P1Y, P2X - P1X).ToSingle();
            }
        }

        #endregion

        #region CONSTRUCTORS

        internal Line(ModalLevel modalLevel, Point point1, Point point2, Color colorLightMode, Color colorDarkMode, int thickness, bool visible = true)
            : base(modalLevel, new Rectangle(point1.X, point1.Y, point2.X - point1.X, thickness))
        {
            P1X = OriginalP1X = point1.X;
            P1Y = OriginalP1Y = point1.Y;
            P2X = OriginalP2X = point2.X;
            P2Y = OriginalP2Y = point2.Y;
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
                Screen.SpriteBatch.DrawLine(this, ColorMode, CurrentTransparency, Thickness);
        }

        #endregion
    }
}
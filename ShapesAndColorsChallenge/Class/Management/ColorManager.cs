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
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class ColorManager
    {
        #region CONST

        internal static readonly Color WindowBodyColorLightMode = new(255, 255, 255, 255);
        internal static readonly Color WindowBodyColorDarkMode = new(30, 30, 30, 255);

        static readonly Color ButtonBodyColorLightMode = new(255, 255, 255, 255);
        static readonly Color ButtonBodyColorDarkMode = new(30, 30, 30, 255);

        internal static readonly Color LinkLightMode = Color.Blue;
        internal static readonly Color LinkDarkMode = new(62, 166, 255, 255);

        internal static readonly Color VersionLightMode = Color.Red;
        internal static readonly Color VersionDarkMode = new(255, 102, 102, 255);

        internal static readonly Color LightGray = Color.LightGray;
        internal static readonly Color MediumGray = Color.DarkGray;
        internal static readonly Color HardGray = Color.Gray;
        internal static readonly Color VeryHardGray = Color.DimGray;

        internal static readonly Color CheckBoxLightMode = Color.DimGray;
        internal static readonly Color CheckBoxDarkMode = Color.White;

        #region SHAPE COLOR

        internal static readonly Color Red = new(255, 0, 0, 255);
        internal static readonly Color Blue = new(0, 0, 255, 255);
        internal static readonly Color Green = new(0, 153, 0, 255);
        internal static readonly Color Yellow = new(253, 232, 0, 255);
        internal static readonly Color Orange = new(255, 163, 0, 255);
        internal static readonly Color Purple = new(153, 0, 255, 255);
        internal static readonly Color Pink = new(255, 200, 255, 255);
        internal static readonly Color Brown = new(144, 84, 45, 255);
        internal static readonly Color Black = new(0, 0, 0, 255);
        internal static readonly Color Gray = new(153, 153, 153, 255);
        internal static readonly Color Cyan = new(0, 255, 255, 255);
        internal static readonly Color LightGreen = new(0, 255, 0, 255);
        internal static readonly Color Turquoise = new(0, 155, 210, 255);
        internal static readonly Color Magenta = new(255, 0, 255, 255);

        #endregion

        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        /// <summary>
        /// Devuelve el color de fondo de las ventanas.
        /// </summary>
        internal static Color WindowBodyColor
        {
            get
            {
                return Statics.IsDarkModeActive ? WindowBodyColorDarkMode : WindowBodyColorLightMode;
            }
        }

        /// <summary>
        /// Devuelve el color de fondo de las ventanas al revés.
        /// </summary>
        internal static Color WindowBodyColorInverted
        {
            get
            {
                return Statics.IsDarkModeActive ? WindowBodyColorLightMode : WindowBodyColorDarkMode;
            }
        }

        internal static Color WindowBorderColor
        {
            get
            {
                return Statics.IsDarkModeActive ? LightGray : VeryHardGray;
            }
        }

        internal static Color VersionColor
        {
            get
            {
                return Statics.IsDarkModeActive ? VersionDarkMode : VersionLightMode;
            }
        }

        internal static Color ButtonBorderColor
        {
            get
            {
                return Statics.IsDarkModeActive ? LightGray : VeryHardGray;
            }
        }

        internal static Color ButtonBodyColor
        {
            get
            {
                return Statics.IsDarkModeActive ? ButtonBodyColorDarkMode : ButtonBodyColorLightMode;
            }
        }

        internal static Color CheckBoxColor
        {
            get
            {
                return Statics.IsDarkModeActive ? CheckBoxDarkMode : CheckBoxLightMode;
            }
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        internal static Color GetShapeColor(TileColor type)
        {
            return type switch
            {
                TileColor.Red => Red,
                TileColor.Blue => Blue,
                TileColor.Green => Green,
                TileColor.Yellow => Yellow,
                TileColor.Orange => Orange,
                TileColor.Purple => Purple,
                TileColor.Pink => Pink,
                TileColor.Brown => Brown,
                TileColor.Black => Black,
                TileColor.Gray => Gray,
                TileColor.Cyan => Cyan,
                TileColor.LightGreen => LightGreen,
                TileColor.Turquoise => Turquoise,
                TileColor.Magenta => Magenta,
                _ => Color.White,
            };
        }

        #endregion
    }
}
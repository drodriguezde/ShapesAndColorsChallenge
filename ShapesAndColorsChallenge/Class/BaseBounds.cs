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
* DISPOSE CONTROL : STATIC
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Microsoft.Xna.Framework;

namespace ShapesAndColorsChallenge.Class
{
    internal static class BaseBounds
    {
        #region CONST

        /// <summary>
        /// Tamaño de origen para los elementos y su posición.
        /// Se toma de base para otras resoluciones.
        /// </summary>
        internal static readonly Rectangle Bounds = new(0, 0, 1080, 2264);

        /// <summary>
        /// Dendisas de puntos por pulgada base.
        /// </summary>
        internal static readonly float DPI = 440;

        /// <summary>
        /// Límites de la ventana, los elementos deben estar dentro de estos limites.
        /// </summary>
        internal static readonly Rectangle Limits = new(50, 50, 980, 2164);

        /// <summary>
        /// Posición y tamaño de los títulos de las ventanas.
        /// </summary>
        internal static readonly Rectangle Title = new(50, 70, 980, 150);

        /// <summary>
        /// Tamaño de la textura de las fichas.
        /// </summary>
        internal static readonly Size TileSize = new(256, 256);

        /// <summary>
        /// Tamaño de la textura de los perk.
        /// </summary>
        internal static readonly Size PerkSize = new(256, 256);

        /// <summary>
        /// Tamaño de la textura de los perk.
        /// </summary>
        internal static readonly Size ModeImageSize = new(256, 256);

        /// <summary>
        /// Tamaño normal de un checkbox.
        /// </summary>
        internal static readonly Rectangle CheckBox = new(0, 0, 100, 100);

        /// <summary>
        /// Tamaño normal de un botón de la interfaz.
        /// </summary>
        internal static readonly Size Button = new(200, 200);

        /// <summary>
        /// Tamaño normal de un botón de potenciador.
        /// </summary>
        internal static readonly Size Perk = new(256, 256);

        /// <summary>
        /// Tamaño normal de la imagen de un potenciador.
        /// </summary>
        internal static readonly Size PerkImage = new(480, 480);

        #endregion
    }
}
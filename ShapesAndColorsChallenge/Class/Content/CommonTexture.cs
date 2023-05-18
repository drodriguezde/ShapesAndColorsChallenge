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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal class CommonTexture : IDisposable
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

        internal Texture2D Texture { get; set; }

        internal Size Size { get; private set; } = new Size(0, 0);

        internal Color BorderColor { get; private set; } = Color.White;

        internal Color Color { get; private set; } = Color.White;

        internal bool IsCircle { get; private set; } = false;

        internal CommonTextureType CommonTextureType { get; private set; } = CommonTextureType.None;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// No usar para construir texturas, eso se hace en TextureManager.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="color"></param>
        internal CommonTexture(Size size, Color color) : this(size, color, color, CommonTextureType.Rectangle)
        {
        }

        /// <summary>
        /// No usar para construir texturas, eso se hace en TextureManager.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="commonTextureType"></param>
        internal CommonTexture(Size size, Color color, CommonTextureType commonTextureType) : this(size, color, color, commonTextureType)
        {
        }

        /// <summary>
        /// No usar para construir texturas, eso se hace en TextureManager.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="borderColor"></param>
        /// <param name="commonTextureType"></param>
        internal CommonTexture(Size size, Color color, Color borderColor, CommonTextureType commonTextureType)
        {
            Size = size;
            Color = color;
            BorderColor = borderColor;
            CommonTextureType = commonTextureType;
        }

        #endregion

        #region DESTRUCTOR

        /// <summary>
        /// Variable que indica si se ha destruido el objeto.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Libera todos los recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                Texture?.Dispose();
            }

            /*Objetos no administrados aquí*/


            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~CommonTexture()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS        



        #endregion
    }
}
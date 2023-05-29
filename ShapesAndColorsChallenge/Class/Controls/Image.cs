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

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Image : InteractiveObject, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region PROPERTIES

        internal Texture2D Texture { get; set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="texture"></param>
        internal Image(ModalLevel modalLevel, Rectangle bounds, Texture2D texture)
            : base(modalLevel, bounds)
        {
            Texture = texture;
            Align();
        }

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="texture"></param>
        internal Image(ModalLevel modalLevel, Rectangle bounds, Texture2D texture, Color color, bool fitInBounds, int fitInBoundsOffset = 0, bool squared = true)
            : base(modalLevel, bounds)
        {
            Texture = texture;
            ColorLightMode = color;
            ColorDarkMode = color;
            FitInBounds(fitInBounds, fitInBoundsOffset, squared);
            Align();
        }

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="texture"></param>
        /// <param name="colorLightMode"></param>
        /// <param name="colorDarkMode"></param>
        /// <param name="fitInBounds"></param>
        /// <param name="fitInBoundsOffset"></param>
        /// <param name="squared">Indica se respeta la proporción o se hace a la medida más corta, un cuadrado</param>
        internal Image(ModalLevel modalLevel, Rectangle bounds, Texture2D texture, Color colorLightMode, Color colorDarkMode, bool fitInBounds, int fitInBoundsOffset = 0, bool squared = true)
            : base(modalLevel, bounds)
        {
            Texture = texture;
            ColorLightMode = colorLightMode;
            ColorDarkMode = colorDarkMode;
            FitInBounds(fitInBounds, fitInBoundsOffset, squared);
            Align();
        }

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="texture"></param>
        /// <param name="fitInBounds"></param>
        /// <param name="fitInBoundsOffset"></param>
        /// <param name="squared">Indica se respeta la proporción o se hace a la medida más corta, un cuadrado</param>
        internal Image(ModalLevel modalLevel, Rectangle bounds, Texture2D texture, bool fitInBounds, int fitInBoundsOffset = 0, bool squared = true)
            : base(modalLevel, bounds)
        {
            Texture = texture;
            FitInBounds(fitInBounds, fitInBoundsOffset, squared);
            Align();
        }

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="texture"></param>
        internal Image(ModalLevel modalLevel, Rectangle bounds, Texture2D texture, float rotation, Vector2 origin, bool fitInBounds, int fitInBoundsOffset = 0, bool squared = true)
            : base(modalLevel, bounds)
        {
            Texture = texture;
            Rotation = rotation;
            Origin = origin;
            FitInBounds(fitInBounds, fitInBoundsOffset, squared);
            Align();
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
        internal new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {

            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Image()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        void FitInBounds(bool fitInBounds, int fitInBoundsOffset, bool squared = true)
        {
            if (!fitInBounds)
                return;

            Vector2 newBounds;

            if (squared)
                newBounds = new(Math.Min(Bounds.Width, Bounds.Height) - fitInBoundsOffset, Math.Min(Bounds.Width, Bounds.Height) - fitInBoundsOffset);
            else
                newBounds = new(Bounds.Width - fitInBoundsOffset, Bounds.Height - fitInBoundsOffset);

            Scale = Screen.GetScaleToFit(Texture.Bounds.Size.ToVector2(), newBounds);
        }

        void Align()
        {
            Location = Bounds.CenterMe(Texture.Width * CurrentScale.X, Texture.Height * CurrentScale.Y).Location.ToVector2();
        }

        internal override void LoadContent()
        {
            base.LoadContent();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (Visible)
                Screen.SpriteBatch.Draw(Texture, Location.Redim(), null, ColorMode * CurrentTransparency, Rotation, Origin, CurrentScale, SpriteEffects.None, 0f);

            base.Draw(gameTime);
        }

        #endregion
    }
}
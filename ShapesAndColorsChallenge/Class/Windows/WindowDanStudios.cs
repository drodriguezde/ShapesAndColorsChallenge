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
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowDanStudios : Window, IDisposable
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



        #endregion

        #region CONSTRUCTORS

        internal WindowDanStudios()
            : base(ModalLevel.Window, BaseBounds.Bounds, WindowType.DanStudios)
        {

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
                GameContent.ResetContentImage();/*Lo descargamos, para quitar la imagen del logo, no se va a usar más*/
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowDanStudios()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetLogo();
        }

        void SetLogo()
        {
            Rectangle bounds = new Rectangle(
                BaseBounds.Bounds.Width.Half() - TextureManager.TextureLogo.Width.Half(),
                BaseBounds.Bounds.Height.Half() - TextureManager.TextureLogo.Height.Half(),
                TextureManager.TextureLogo.Width,
                TextureManager.TextureLogo.Height);
            Image imageLogo = new(ModalLevel, bounds, TextureManager.TextureLogo);
            InteractiveObjectManager.Add(imageLogo);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
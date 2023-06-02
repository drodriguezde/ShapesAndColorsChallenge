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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Button : InteractiveObject, IDisposable
    {
        #region CONST

        /// <summary>
        /// Tiempo en milisegundos que será visible el feedback visual para el usuario.
        /// </summary>
        const int TIME_VISUAL_FEEDBACK = 500;

        #endregion

        #region DELEGATES



        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        Texture2D BodyTexture;/*No hay que hacer dispose*/

        public CommonTextureType CommonTextureType { get; set; } = CommonTextureType.RoundedRectangle;

        /// <summary>
        /// Indica que es visible pero no debe dibujarse.
        /// Se usa por si se quiere hacer click en él pero sin textura.
        /// </summary>
        internal bool IsTransparent { get; set; } = false;

        /// <summary>
        /// Indica que se ha cambiado la textura a una que identifique un click.
        /// </summary>
        bool ClickedTexture { get; set; } = false;

        /// <summary>
        /// Indica si hay que mostrar al usuario algún tipo de feedback visual por pulsar.
        /// </summary>
        internal bool DoVisualClickedFeedback { get; set; } = true;

        TimeSpan VisualClickedFeedbackTime { get; set; } = TimeSpan.Zero;

        #endregion

        #region CONSTRUCTORS

        internal Button(ModalLevel modalLevel, Rectangle bounds)
            : base(modalLevel, bounds)
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

            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Button()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetColorMode();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.ButtonBodyColor, ColorManager.ButtonBorderColor, CommonTextureType).Texture;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ClickedRaised && !ClickedTexture && DoVisualClickedFeedback && Visible && DoVisualClickedFeedback)
            {
                ClickedTexture = true;/*Para que lo haga una sola vez*/
                VisualClickedFeedbackTime = gameTime.TotalGameTime;
                BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.ButtonBodyColor, ColorManager.Cyan, CommonTextureType).Texture;
            }

            if (ClickedTexture)/*Para volver a la textura original por si se ha lanzado un popup o similar*/
            {
                if (gameTime.TotalGameTime.Subtract(VisualClickedFeedbackTime).TotalMilliseconds < TIME_VISUAL_FEEDBACK)
                    return;

                ClickedTexture = false;
                ClickedRaised = false;
                SetColorMode();
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            if ((Visible && !IsTransparent) || (Visible && DoVisualClickedFeedback && ClickedTexture && IsTransparent/*Para mostrar el efecto click cuando estransparente*/))
                Screen.SpriteBatch.Draw(BodyTexture, Location, Color.White * CurrentTransparency);

            base.Draw(gameTime);
        }

        #endregion
    }
}
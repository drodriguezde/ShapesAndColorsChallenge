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
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    public class WindowSelectRanking : Window, IDisposable
    {
        #region DELEGATES

        internal event EventHandler OnCancel;
        internal event EventHandler OnLocalRanking;
        internal event EventHandler OnGlobalRanking;

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonCancel;
        Button buttonLocal;
        Button buttonGlobal;

        /// <summary>
        /// Tamaño y posición de botoón ok cuando está junto con el botón cancelar para la resolución base.
        /// </summary>
        Rectangle buttonCancelBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() - 306.Half(), BaseBounds.Bounds.Height.Half() + 50, 306, 256).Redim();

        Rectangle buttonLocalBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() - 306, BaseBounds.Bounds.Height.Half() - 275, 256, 256).Redim();

        Rectangle buttonGlobalBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() + 50, BaseBounds.Bounds.Height.Half() - 275, 256, 256).Redim();

        static Rectangle messageBoxBounds = new Rectangle(BaseBounds.Limits.X + 100, BaseBounds.Bounds.Height.Half() - 700.Half(), BaseBounds.Limits.Width - 200, 700).Redim();/*Tiene que ser estática*/


        #endregion

        #region PROPERTIES


        #endregion

        #region CONSTRUCTORS

        internal WindowSelectRanking() : base(ModalLevel.MessageBox, messageBoxBounds, WindowType.Reward)
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
                UnsubscribeEvents();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowSelectRanking()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Engancha los eventos de los objetos interactivos.
        /// </summary>
        void SubscribeEvents()
        {
            if(buttonCancel != null)
                buttonCancel.OnClick += ButtonCancel_OnClick;

            if (buttonLocal != null)
                buttonLocal.OnClick += ButtonLocal_OnClick;

            if (buttonGlobal != null)
                buttonGlobal.OnClick += ButtonGlobal_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            if (buttonCancel != null)
                buttonCancel.OnClick -= ButtonCancel_OnClick;

            if (buttonLocal != null)
                buttonLocal.OnClick -= ButtonLocal_OnClick;

            if (buttonGlobal != null)
                buttonGlobal.OnClick -= ButtonGlobal_OnClick;
        }

        void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            OnCancel?.Invoke(sender, e);
        }

        void ButtonLocal_OnClick(object sender, EventArgs e)
        {
            OnLocalRanking?.Invoke(sender, e);
        }

        void ButtonGlobal_OnClick(object sender, EventArgs e)
        {
            OnGlobalRanking?.Invoke(sender, e);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetColorMode();
            SetBackLayer();
            SetButtonCancel();
            SetButtonLocal();
            SetButtonGlobal();
            SubscribeEvents();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.WindowBodyColor, ColorManager.WindowBorderColor, CommonTextureType.RoundedRectangle).Texture;
        }

        void SetBackLayer()
        {
            TextureBackLayer = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), Color.Black, CommonTextureType.Rectangle).Texture;
        }
            
        void SetButtonCancel()
        {
            buttonCancel = new Button(ModalLevel, buttonCancelBounds);
            Image imageCancel = new(ModalLevel, buttonCancelBounds, TextureManager.TextureCancelButton, ColorManager.HardGray, ColorManager.HardGray, true, 20, true);
            InteractiveObjectManager.Add(buttonCancel, imageCancel);
        }

        void SetButtonLocal()
        {
            buttonLocal = new Button(ModalLevel, buttonLocalBounds);
            Image imageLocal = new(ModalLevel, buttonLocalBounds, TextureManager.TextureRankingLocal, ColorManager.HardGray, ColorManager.HardGray, true, 20, true);
            InteractiveObjectManager.Add(buttonLocal, imageLocal);
        }

        void SetButtonGlobal()
        {
            buttonGlobal = new Button(ModalLevel, buttonGlobalBounds);
            Image imageGlobal = new(ModalLevel, buttonGlobalBounds, TextureManager.TextureRankingGlobal, ColorManager.HardGray, ColorManager.HardGray, true, 20, true);
            InteractiveObjectManager.Add(buttonGlobal, imageGlobal);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            Screen.SpriteBatch.Draw(TextureBackLayer, Screen.Bounds, Color.White * BacklayerTransparency);
            Screen.SpriteBatch.Draw(BodyTexture, Location, Color.White * BodyTransparency);
            base.Draw(gameTime);
        }

        #endregion
    }
}
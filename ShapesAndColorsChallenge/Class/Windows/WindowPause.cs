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
    public class WindowPause : Window, IDisposable
    {
        #region IMPORTS



        #endregion

        #region DELEGATES

        internal event EventHandler OnResume;
        internal event EventHandler OnQuit;

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonResume;
        Button buttonQuit;
        Label labelMessage;
        static Rectangle messageBoxBounds = new(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height.Half() - 950.Half(), BaseBounds.Limits.Width - 100, 880);/*Tiene que ser estática*/
        Rectangle titleBounds = new(170, 690, 700, 150);
        Rectangle infoChallengeLabel = new(BaseBounds.Limits.X + 100, 830, BaseBounds.Limits.Width - 200, 100);

        /// <summary>
        /// Tamaño y posición de botón continuar.
        /// </summary>
        Rectangle buttonResumeBounds = new(BaseBounds.Limits.X + 100, 960, BaseBounds.Limits.Width - 200, BaseBounds.Button.Height);

        /// <summary>
        /// Tamaño y posición de botón abandonar.
        /// </summary>
        Rectangle buttonQuitBounds = new(BaseBounds.Limits.X + 100, 1250, BaseBounds.Limits.Width - 200, BaseBounds.Button.Height);

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Indica si se está jugado un reto.
        /// </summary>
        bool IsInChallenge { get; set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isInChallenge">Indica si se está jugado un reto.</param>
        internal WindowPause(bool isInChallenge) : base(ModalLevel.MessageBox, messageBoxBounds, WindowType.MessageBox)
        {
            IsInChallenge = isInChallenge;
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
        ~WindowPause()
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
            if (buttonResume != null)
                buttonResume.OnClick += ButtonResume_OnClick;

            if (buttonQuit != null)
                buttonQuit.OnClick += ButtonQuit_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            if (buttonResume != null)
                buttonResume.OnClick -= ButtonResume_OnClick;

            if (buttonQuit != null)
                buttonQuit.OnClick -= ButtonQuit_OnClick;
        }

        private void ButtonResume_OnClick(object sender, EventArgs e)
        {
            OnResume?.Invoke(sender, e);
        }

        private void ButtonQuit_OnClick(object sender, EventArgs e)
        {
            OnQuit?.Invoke(sender, e);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetBody();
            SetColorMode();
            SetBackLayer();
            InitializeButtons();
            SetTitle();
            SetInfoChallenge();
            SubscribeEvents();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.WindowBodyColor, ColorManager.WindowBorderColor, CommonTextureType.RoundedRectangle).Texture;
        }

        void SetBody()
        {
            BodyBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
        }

        void SetBackLayer()
        {
            TextureBackLayer = TextureManager.Get(new Size(1, 1), Color.Black, CommonTextureType.Rectangle).Texture;
        }

        void InitializeButtons()
        {
            buttonResume = new Button(ModalLevel, buttonResumeBounds);
            buttonQuit = new Button(ModalLevel, buttonQuitBounds);
            Label labelResume = new(ModalLevel, new(buttonResumeBounds.X, buttonResumeBounds.Y + 20, buttonResumeBounds.Width, 160), Resource.String.RESUME.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Label labelQuit = new(ModalLevel, new(buttonQuitBounds.X, buttonQuitBounds.Y + 20, buttonQuitBounds.Width, 160), Resource.String.QUIT.GetString(), Color.OrangeRed, Color.OrangeRed, AlignHorizontal.Center);
            InteractiveObjectManager.Add(buttonResume, buttonQuit, labelResume, labelQuit);
        }

        void SetTitle()
        {
            labelMessage = new Label(ModalLevel, titleBounds, Resource.String.PAUSE.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelMessage);
        }

        void SetInfoChallenge()
        {
            if (!IsInChallenge)
                return;

            Label label = new(ModalLevel, infoChallengeLabel, Resource.String.CHALLENGE_LEAVE.GetString(), Color.Red, Color.Red, AlignHorizontal.Center);
            InteractiveObjectManager.Add(label);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            Screen.SpriteBatch.Draw(TextureBackLayer, Screen.BacklayerBounds, Color.White * BacklayerTransparency);
            Screen.SpriteBatch.Draw(BodyTexture, Location, Color.White * BodyTransparency);
            base.Draw(gameTime);
        }

        #endregion
    }
}
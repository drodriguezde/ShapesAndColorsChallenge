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
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowHowToPlay : Window, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS        

        NavigationPanelHorizontal navigationPanelHorizontal;
        CheckBox checkBoxHowToPlay;
        Button buttonPlay;

        readonly Rectangle buttonPlayBounds = new Rectangle(
            BaseBounds.Limits.X + (BaseBounds.Limits.Width - BaseBounds.Button.Width * 4) + BaseBounds.Button.Width.Multi(3),
            BaseBounds.Limits.Bottom - BaseBounds.Button.Height,
            BaseBounds.Button.Width,
            BaseBounds.Button.Height);

        #endregion

        #region PROPERTIES

        WindowHowToPlayParams WindowHowToPlayParams { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowHowToPlay(WindowHowToPlayParams parameters)
            : base(ModalLevel.Window, BaseBounds.Bounds, WindowType.HowToPlay)
        {
            WindowHowToPlayParams = parameters;
            AddBackButton = WindowHowToPlayParams.OriginInfoButton;
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
        ~WindowHowToPlay()
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
            if (WindowHowToPlayParams.OriginInfoButton)
                return;

            checkBoxHowToPlay.OnCheckedChange += CheckBoxHowToPlay_OnCheckedChange;
            buttonPlay.OnClick += ButtonPlay_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            if (WindowHowToPlayParams.OriginInfoButton)
                return;

            checkBoxHowToPlay.OnCheckedChange -= CheckBoxHowToPlay_OnCheckedChange;
            buttonPlay.OnClick -= ButtonPlay_OnClick;
        }

        private void CheckBoxHowToPlay_OnCheckedChange(object sender, EventArgs e)
        {
            ControllerSettings.SetHowToPlay(OrchestratorManager.GameMode, (sender as CheckBox).Checked);
        }

        private void ButtonPlay_OnClick(object sender, EventArguments.OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Game);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetPanel();
            SetTitle();
            SetInfo();
            SetButtonPlay();
            SetCheckBox();
            SubscribeEvents();
            navigationPanelHorizontal.Set();
        }

        void SetPanel()
        {
            navigationPanelHorizontal = new(ModalLevel, Bounds, BaseBounds.Limits.Bottom - BaseBounds.Button.Height - 150, this);
            InteractiveObjectManager.Add(navigationPanelHorizontal);
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, string.Concat(Resource.String.HOW_TO_PLAY.GetString(), " (", Statics.GetGameModeTitle(), ")"), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTitle);
        }

        void SetInfo()
        {
            Image imageMode = new(ModalLevel, new(BaseBounds.Limits.X, BaseBounds.Title.Y + 150, BaseBounds.Limits.Width, 1400), Statics.GetHowToPlayTexture(OrchestratorManager.GameMode), Color.White, Color.White, true, 0, false);
            Label labelDescription = new(ModalLevel, new(BaseBounds.Limits.X, BaseBounds.Title.Y + 1450, BaseBounds.Limits.Width, 400), Statics.GetHowToPlayDescription(OrchestratorManager.GameMode), Color.Gray, Color.Gray, AlignHorizontal.Left, (Statics.GetHowToPlayDescription(OrchestratorManager.GameMode).Length / 40f).Ceiling());
            Rectangle bounds = new(BaseBounds.Limits.X, BaseBounds.Title.Y + 300, 380, 380);
            Image imageTimeStop = new(ModalLevel, bounds, TextureManager.TexturePerkTimeStop, Color.Gray, Color.Gray, true, 0, true);
            Label labelTimeStop = new(ModalLevel, new(bounds.X + bounds.Width + 50, bounds.Top, BaseBounds.Bounds.Width - (bounds.X + bounds.Width + 150), bounds.Height), Resource.String.PERK_TIMESTOP.GetString(), Color.Gray, Color.Gray, AlignHorizontal.Left, (Resource.String.PERK_TIMESTOP.GetString().Length / 25f).Ceiling());
            bounds = new(BaseBounds.Limits.X, BaseBounds.Title.Y + 400 + 380, 380, 380);
            Image imageReveal = new(ModalLevel, bounds, TextureManager.TexturePerkReveal, Color.Gray, Color.Gray, true, 0, true);
            Label labelReveal = new(ModalLevel, new(bounds.X + bounds.Width + 50, bounds.Top, BaseBounds.Bounds.Width - (bounds.X + bounds.Width + 150), bounds.Height), Resource.String.PERK_REVEAL.GetString(), Color.Gray, Color.Gray, AlignHorizontal.Left, (Resource.String.PERK_REVEAL.GetString().Length / 25f).Ceiling());
            bounds = new(BaseBounds.Limits.X, BaseBounds.Title.Y + 500 + 760, 380, 380);
            Image imageChange = new(ModalLevel, bounds, TextureManager.TexturePerkChange, Color.Gray, Color.Gray, true, 0, true);
            Label labelChange = new(ModalLevel, new(bounds.X + bounds.Width + 50, bounds.Top, BaseBounds.Bounds.Width - (bounds.X + bounds.Width + 150), bounds.Height), Resource.String.PERK_CHANGE.GetString(), Color.Gray, Color.Gray, AlignHorizontal.Left, (Resource.String.PERK_CHANGE.GetString().Length / 25f).Ceiling());
            InteractiveObjectManager.Add(imageMode, labelDescription, imageTimeStop, labelTimeStop, imageReveal, labelReveal, imageChange, labelChange);
            navigationPanelHorizontal.Add(1, imageMode, labelDescription);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
            navigationPanelHorizontal.Add(2, imageTimeStop, labelTimeStop);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
            navigationPanelHorizontal.Add(2, imageReveal, labelReveal);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
            navigationPanelHorizontal.Add(2, imageChange, labelChange);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetButtonPlay()
        {
            if (WindowHowToPlayParams.OriginInfoButton)
                return;

            buttonPlay = new Button(ModalLevel, buttonPlayBounds);
            Image image = new(ModalLevel, buttonPlayBounds, TextureManager.TexturePlay, Color.DarkGray, Color.DarkGray, true, 25);
            InteractiveObjectManager.Add(buttonPlay, image);
        }

        void SetCheckBox()
        {
            if (WindowHowToPlayParams.OriginInfoButton)
                return;

            Rectangle bounds = new(BaseBounds.Limits.X, BaseBounds.Limits.Bottom - BaseBounds.Button.Height.Half() - BaseBounds.CheckBox.Height.Half(), BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
            checkBoxHowToPlay = new(ModalLevel, bounds, false);
            InteractiveObjectManager.Add(checkBoxHowToPlay, checkBoxHowToPlay.GetLabel(Resource.String.DONT_SHOW_AGAIN));
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
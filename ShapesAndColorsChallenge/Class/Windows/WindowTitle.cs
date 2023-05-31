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

using Android.App;
using Android.Content;
using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Particles;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowTitle : Window, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Link linkToDanSite;/*Al tener eventos debe estar aquí declarada*/
        Button playButton;
        AnimationHeartBeat animationHeartBeat;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Posición y tamaño para la resolución base.
        /// Se recalculará.
        /// </summary>
        Rectangle ButtonPlayBounds { get; set; } = new Rectangle(BaseBounds.Limits.X, 770, BaseBounds.Limits.Width, BaseBounds.Bounds.Height - 770.Double());

        /// <summary>
        /// Tamaño y posición inicial de la primera linea de título de la aplicación para la resolución base.
        /// </summary>
        Rectangle TitleFirstLineBounds { get; set; } = new Rectangle(BaseBounds.Limits.X, 330, BaseBounds.Limits.Width, 250);

        /// <summary>
        /// Tamaño y posición inicial de la segunda linea de título de la aplicación para la resolución base.
        /// </summary>
        Rectangle TitleSecondLineBounds { get; set; } = new Rectangle(BaseBounds.Limits.X, 480, BaseBounds.Limits.Width, 250);

        Rectangle LinkBounds { get; set; } = new Rectangle(675, BaseBounds.Limits.Bottom - 180, 250, 80);

        Rectangle LabelVersionBounds { get; set; } = new Rectangle(650, BaseBounds.Limits.Bottom - 90, 300, 90);

        Rectangle PlayTextBounds = new Rectangle(BaseBounds.Limits.X, BaseBounds.Bounds.Height.Half() - 150.Half(), BaseBounds.Limits.Width, 150);

        #endregion

        #region CONSTRUCTORS

        internal WindowTitle()
            : base(ModalLevel.Window, BaseBounds.Bounds, WindowType.Title)
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
                Nuller.Null(ref animationHeartBeat);
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowTitle()
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
            linkToDanSite.OnClick += LinkToDanSite_OnClick;
            playButton.OnClick += ButtonPlay_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            linkToDanSite.OnClick -= LinkToDanSite_OnClick;
            playButton.OnClick -= ButtonPlay_OnClick;
        }

        /// <summary>
        /// Cuando se dispara el evento Click del link acaba aquí.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LinkToDanSite_OnClick(object sender, OnClickEventArgs e)
        {
            var uri = Android.Net.Uri.Parse(Const.DAN_SITE_URL);
            var intent = new Intent(Intent.ActionView, uri);
            intent.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }

        void ButtonPlay_OnClick(object sender, OnClickEventArgs e)
        {
            ParticleEngine.End();
            CloseMeAndOpenThis(WindowType.GameMode);
        }

        #endregion

        #region METHODS

        void InitializeDanLink()
        {
            linkToDanSite = new(ModalLevel, LinkBounds, true, this);
            InteractiveObjectManager.Add(linkToDanSite);
            Label labelTextLink = new(ModalLevel, LinkBounds, Resource.String.MORE_GAMES.GetString(), ColorManager.LinkLightMode, ColorManager.LinkDarkMode, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTextLink);
        }

        void InitializeAppVersion()
        {
            Label labelVersion = new(ModalLevel, LabelVersionBounds, string.Concat(Resource.String.VERSION.GetString(), ": ", Statics.GetAppVersion()), ColorManager.VersionLightMode, ColorManager.VersionDarkMode, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelVersion);
        }

        void InitializePlayText()
        {
            Label labelPlay = new(ModalLevel, PlayTextBounds, Resource.String.PLAY.GetString(), Color.Orange, Color.Orange, AlignHorizontal.Center) { LockScaleToFit = false };
            InteractiveObjectManager.Add(labelPlay);
            /*La inicialización de la animación debe ir después de InteractiveObjectManager.Add para que se construya completamente el objeto a animar*/
            animationHeartBeat = new AnimationHeartBeat(labelPlay, 1200) { ScaleHeartBeat = 2f, FramesOfTheAnimation = 10 };
            animationHeartBeat.Start();
        }

        void InitializeTitle()
        {
            Label labelFirstLineTitle = new(ModalLevel, TitleFirstLineBounds, string.Concat(Resource.String.APP_TITLE_FIRST_LINE.GetString()), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelFirstLineTitle);

            Label labelSecondLineTitle = new(ModalLevel, TitleSecondLineBounds, string.Concat(Resource.String.APP_TITLE_SECOND_LINE.GetString()), ColorManager.VeryHardGray, ColorManager.VeryHardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelSecondLineTitle);
        }

        void InitializePlayButton()
        {
            playButton = new Button(ModalLevel, ButtonPlayBounds) { IsTransparent = true, DoVisualClickedFeedback = false };
            InteractiveObjectManager.Add(playButton);
        }

        internal override void LoadContent()
        {
            base.LoadContent();
            InitializeDanLink();
            InitializeAppVersion();
            InitializeTitle();
            InitializePlayButton();
            InitializePlayText();
            SubscribeEvents();
            ParticleEngine.Start(ParticleType.Falling);
        }

        internal override void Update(GameTime gameTime)
        {
            animationHeartBeat.Update(gameTime);
            base.Update(gameTime);
            ParticleEngine.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);/*Tiene que ir antes*/
            ParticleEngine.Draw(gameTime);
        }

        #endregion
    }
}
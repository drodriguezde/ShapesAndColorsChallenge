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
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    public class Window : Entity, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES

        internal event EventHandler OnClose;
        internal event EventHandler OnBack;
        internal event EventHandler<OnFinishTransitionEventArgs> OnFinishTransition;

        #endregion

        #region VARS

        /// <summary>
        /// Ubicación y tamaño del botón volver.
        /// </summary>
        Rectangle buttonBackBounds = new(BaseBounds.Limits.X, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Button.Width, BaseBounds.Button.Height);

        InteractiveObjectManager interactiveObjectManager;
        Button buttonBack;

        #endregion

        #region PROPERTIES

        internal WindowType WindowType { get; set; } = WindowType.None;

        internal InteractiveObjectManager InteractiveObjectManager
        {
            get => interactiveObjectManager;
            set => interactiveObjectManager = value;
        }

        internal Rectangle BodyBounds { get; set; }

        internal Texture2D BodyTexture { get; set; }

        internal float BodyTransparency { get; set; } = 1f;

        internal Texture2D TextureBackLayer { get; set; }

        internal float BacklayerTransparency { get; set; } = 0.3f;

        /// <summary>
        /// Indica la transición que se está llevando a cabo.
        /// </summary>
        TransitionType TransitionType { get; set; }

        /// <summary>
        /// Indica la cantidad de tiempo en milisegundos que dura la transacción.
        /// </summary>
        int TransitionTime { get; set; }

        /// <summary>
        /// Tiempo total transcurrido.
        /// </summary>
        long TotalTime { get; set; }

        /// <summary>
        /// Indica cuando va a aumentar o disminuir la transparencia o la opacidad.
        /// </summary>
        float DiffTransparency { get; set; } = 0f;

        /// <summary>
        /// Indica la transparencia maestra que deberían tener los objetos de la ventana.
        /// </summary>
        float TransitionTransparency { get; set; } = 0f;

        /// <summary>
        /// Indica que hay que crear el botón de volver.
        /// </summary>
        internal bool AddBackButton { get; set; } = true;

        /// <summary>
        /// Indica que hay que crear un fondo de color para las ventanas que tienen panel de desplazamiento horizontal.
        /// </summary>
        internal bool AddBottomBackGround { get; set; } = false;

        /// <summary>
        /// Indica si el botón de volver atrás está bloqueado.
        /// </summary>
        internal bool BlockBack { get; set; } = false;

        internal InteractiveObject PostObject { get; set; } = null;

        #endregion

        #region CONSTRUCTORS

        internal Window(ModalLevel modalLevel, Rectangle bounds, WindowType windowType)
            : base(modalLevel, bounds)
        {
            WindowType = windowType;
            InteractiveObjectManager = new InteractiveObjectManager();
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
                Nuller.Null(ref interactiveObjectManager);
                UnsubscribeEvents();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Window()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void OnClose_Raised(object sender, EventArgs e)
        {
            OnClose?.Invoke(sender, e);
        }

        void ButtonBack_OnClick(object sender, OnClickEventArgs e)
        {
            if (BlockBack)
                return;

            OnBack?.Invoke(sender, e);
            ExitManager.BackButtonPressed = true;
        }

        #endregion

        #region METHODS

        void SubscribeEvents()
        {
            if (AddBackButton && buttonBack != null)
                buttonBack.OnClick += ButtonBack_OnClick;
        }

        void UnsubscribeEvents()
        {
            if (AddBackButton && buttonBack != null)
                buttonBack.OnClick += ButtonBack_OnClick;
        }

        internal virtual void CloseMeAndOpenThis(WindowType windowTypeDestination, object windowParams = null)
        {
            OrchestratorManager.StartTransition(this, windowTypeDestination, windowParams);
        }

        internal override void LoadContent()
        {
            SetBackButton();
            AddPostObject();
        }

        void SetBackButton()
        {
            if (!AddBackButton)
                return;

            if (WindowType == WindowType.Game)/*Es el botón de pausa en esta ventana*/
                return;

            SetBottomBackGround();
            buttonBack = new(ModalLevel, buttonBackBounds);
            Image imageBack = new(ModalLevel, buttonBackBounds, TextureManager.TextureBackButton, Color.DarkGray, Color.DarkGray, true);
            InteractiveObjectManager.Add(buttonBack, imageBack);
            SubscribeEvents();
        }

        /// <summary>
        /// Sirva para poner algún objeto encima de la imageBack inferior.
        /// </summary>
        void AddPostObject()
        {
            if (PostObject != null)
                interactiveObjectManager.Add(PostObject);
        }

        void SetBottomBackGround()
        {
            if (!AddBottomBackGround)
                return;

            Image backButtonBackground = new(
                ModalLevel,
                new(0, buttonBackBounds.Y - 20, BaseBounds.Bounds.Width, BaseBounds.Bounds.Height - buttonBackBounds.Y + 20),
                TextureManager.Get(new(BaseBounds.Bounds.Width, BaseBounds.Bounds.Height - buttonBackBounds.Y + 20), ColorManager.WindowBodyColor, CommonTextureType.Rectangle).Texture, false)
            { AllowFadeInFadeOut = false };
            InteractiveObjectManager.Add(backButtonBackground);
        }

        /// <summary>
        /// Indica a todos los InteractiveObject de la ventana que comiencen una transición, si están visibles.
        /// </summary>
        internal void StartTransition(TransitionType transitionType, int transitionTime)
        {
            if (!Visible || TransitionType != TransitionType.None/*Esta evita que se repita la transición si se pulsa muy rápido un botón*/)
                return;

            TotalTime = 0;
            TransitionType = transitionType;
            TransitionTime = transitionTime;
            DiffTransparency = 1f / TransitionTime;
            TransitionTransparency = TransitionType == TransitionType.Hide ? 1f : 0f;
            InteractiveObjectManager.SetMasterTransparency(TransitionTransparency);
        }

        internal override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;

            if (TransitionType != TransitionType.None)
                if (DoTransition(gameTime))
                    return;/*Si ha acabado la transición y es un Fade Out para que no continue en update por si se anulado esta ventana y no de un error en InteractiveObjectManager.Update*/

            /*Si ha cambiado el modo de color hay que actualizar los elementos de las ventanas activas*/
            if (IsDarkModeCurrentlyActive != Statics.IsDarkModeActive)
            {
                SetColorMode();
                IsDarkModeCurrentlyActive = Statics.IsDarkModeActive;
            }

            InteractiveObjectManager.Update(gameTime);
        }

        /// <summary>
        /// Hace la transición.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>True si ha finalizado la transición.</returns>
        bool DoTransition(GameTime gameTime)
        {
            TotalTime += gameTime.ElapsedGameTime.Milliseconds;

            if (TransitionType == TransitionType.Hide)
                TransitionTransparency = 1f - (DiffTransparency * TotalTime);
            else
                TransitionTransparency = DiffTransparency * TotalTime;

            if (TransitionTransparency < 0f)
                TransitionTransparency = 0f;

            if (TransitionTransparency > 1f)
                TransitionTransparency = 1f;

            InteractiveObjectManager.SetMasterTransparency(TransitionTransparency);
            InteractiveObjectManager.Update(gameTime);

            /*Cuadno finaliza la transición*/
            if ((TransitionType == TransitionType.Hide && TransitionTransparency == 0f) || (TransitionType == TransitionType.Show && TransitionTransparency == 1f))
            {
                TransitionType currentTransition = TransitionType;
                TransitionType = TransitionType.None;/*Tiene que ir antes*/
                OnFinishTransition?.Invoke(this, new OnFinishTransitionEventArgs(currentTransition));
                return true;
            }

            return false;
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            InteractiveObjectManager.Draw(gameTime);
        }

        #endregion
    }
}
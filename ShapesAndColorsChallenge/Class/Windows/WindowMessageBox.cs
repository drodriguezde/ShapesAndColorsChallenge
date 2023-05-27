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
    public class WindowMessageBox : Window, IDisposable
    {
        #region IMPORTS



        #endregion

        #region DELEGATES

        internal event EventHandler OnAccept;
        internal event EventHandler OnCancel;

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonAccept;
        Button buttonCancel;
        Label labelMessage;

        #endregion

        #region PROPERTIES

        MessageBoxButton MessageBoxButton { get; set; }

        /// <summary>
        /// Devuelve el tamaño y posición del botón aceptar dependiendo de lo seleccionado.
        /// </summary>
        Rectangle AcceptBounds
        {
            get { return MessageBoxButton == MessageBoxButton.Accept ? ButtonAloneBounds : ButtonOKBounds; }
        }

        /// <summary>
        /// Devuelve el tamaño y posición del botón cancelar dependiendo de lo seleccionado.
        /// </summary>
        Rectangle CancelBounds
        {
            get { return MessageBoxButton == MessageBoxButton.Cancel ? ButtonAloneBounds : ButtonCancelBounds; }
        }

        string Message { get; set; } = string.Empty;

        int LinesNumber { get; set; } = 1;

        static Rectangle MessageBoxBounds { get; set; } = new Rectangle(BaseBounds.Limits.X + 50, 840, BaseBounds.Limits.Width - 100, 570).Redim();/*Tiene que ser estática*/

        /// <summary>
        /// Tamaño y posición de botoón ok cuando está junto con el botón cancelar para la resolución base.
        /// </summary>
        Rectangle ButtonOKBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() - 712.Half(), 1082, 306, 256).Redim();

        /// <summary>
        /// Tamaño y posición de botoón cancelar cuando está junto con el botón ok para la resolución base.
        /// </summary>
        Rectangle ButtonCancelBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() + 50, 1082, 306, 256).Redim();

        /// <summary>
        /// Tamaño y posición de botón ok o cancelar cuando está solo.
        /// </summary>
        Rectangle ButtonAloneBounds { get; set; } = new Rectangle(BaseBounds.Bounds.Width.Half() - 306.Half(), 1082, 306, 256).Redim();

        Rectangle MessageBounds { get; set; } = new Rectangle(170, 887, 710, 120).Redim();

        #endregion

        #region CONSTRUCTORS

        internal WindowMessageBox(MessageBoxButton messageBoxButton, string message, int linesNumber = 1)
            : base(ModalLevel.MessageBox, MessageBoxBounds, WindowType.MessageBox)
        {
            MessageBoxButton = messageBoxButton;
            Message = message;
            LinesNumber = linesNumber;
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
        ~WindowMessageBox()
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
            if (buttonAccept != null)
                buttonAccept.OnClick += ButtonAccept_OnClick;

            if (buttonCancel != null)
                buttonCancel.OnClick += ButtonCancel_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            if (buttonAccept != null)
                buttonAccept.OnClick -= ButtonAccept_OnClick;

            if (buttonCancel != null)
                buttonCancel.OnClick -= ButtonCancel_OnClick;
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            OnAccept?.Invoke(sender, e);
        }

        private void ButtonCancel_OnClick(object sender, EventArgs e)
        {
            OnCancel?.Invoke(sender, e);
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
            InitializeMessage();
            SubscribeEvents();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.WindowBodyColor, ColorManager.WindowBorderColor, CommonTextureType.RoundedRectangle).Texture;
        }

        void SetBody()
        {
            if (MessageBoxButton == MessageBoxButton.None)
            {
                BodyBounds = new Rectangle(Bounds.X, BaseBounds.Bounds.Height.Half() - 100, Bounds.Width, 200);
                Bounds = new Rectangle(Bounds.X, BaseBounds.Bounds.Height.Half() - 100, Bounds.Width, 200);
                MessageBounds = new Rectangle(170, BodyBounds.Top + 40, 710, 120).Redim();
            }
            else
                BodyBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
        }

        void SetBackLayer()
        {
            TextureBackLayer = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), Color.Black, CommonTextureType.Rectangle).Texture;
        }

        void InitializeButtons()
        {
            if (MessageBoxButton == MessageBoxButton.AcceptCancel)
            {
                buttonAccept = new Button(ModalLevel, AcceptBounds);
                buttonCancel = new Button(ModalLevel, CancelBounds);
                Image imageAccept = new(ModalLevel, AcceptBounds, TextureManager.TextureOkButton, ColorManager.HardGray, ColorManager.HardGray, true);
                Image imageCancel = new(ModalLevel, CancelBounds, TextureManager.TextureCancelButton, ColorManager.HardGray, ColorManager.HardGray, true, 30);
                InteractiveObjectManager.Add(buttonAccept, buttonCancel, imageAccept, imageCancel);
            }
            else if (MessageBoxButton == MessageBoxButton.Accept)
            {
                buttonAccept = new Button(ModalLevel, ButtonAloneBounds);
                Image imageAccept = new(ModalLevel, ButtonAloneBounds, TextureManager.TextureOkButton, ColorManager.HardGray, ColorManager.HardGray, true);
                InteractiveObjectManager.Add(buttonAccept, imageAccept);
            }
            else if (MessageBoxButton == MessageBoxButton.Cancel)
            {
                buttonCancel = new Button(ModalLevel, ButtonAloneBounds);
                Image imageCancel = new(ModalLevel, ButtonAloneBounds, TextureManager.TextureCancelButton, ColorManager.HardGray, ColorManager.HardGray, true, 30);
                InteractiveObjectManager.Add(buttonCancel, imageCancel);
            }
        }

        void InitializeMessage()
        {
            labelMessage = new Label(ModalLevel, MessageBounds, Message, ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center, LinesNumber);
            InteractiveObjectManager.Add(labelMessage);
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
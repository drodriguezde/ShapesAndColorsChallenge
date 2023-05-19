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
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class InteractiveObject : Entity, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES

        internal event EventHandler OnHover;
        internal event EventHandler OnLeave;
        internal event EventHandler<OnClickEventArgs> OnClick;
        internal event EventHandler<OnDragEventArgs> OnDrag;
        internal event EventHandler<OnDropEventArgs> OnDrop;

        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        /// <summary>
        /// Indica la tranparencia que tiene mayor peso que la transparencia de los objetos hijo.
        /// Se usa para modificar la transparencia cuando hay alguna animación como fade-in o fade-out.
        /// </summary>
        internal float MasterTransparency { get; set; } = 1f;

        internal float Transparency { get; set; } = 1f;

        /// <summary>
        /// Indica la escala que tiene mayor peso que la escala de los objetos hijo.
        /// Se usa para modificar la escala cuando hay alguna animación.
        /// </summary>
        internal Vector2 MasterScale { get; set; } = Vector2.One;

        internal Vector2 Scale { get; set; } = Vector2.One;

        internal bool IsCircle { get; set; }

        /// <summary>
        /// Rotación del objeto sobre su eje (RADIANES).
        /// </summary>
        internal float Rotation { get; set; } = 0f;

        /// <summary>
        /// Posición de origen de una imagen antes de ser transformada.
        /// </summary>
        internal Vector2 Origin { get; set; } = Vector2.Zero;

        /// <summary>
        /// Como el campo tag en winforms, sirve para meter cualquier cosa.
        /// </summary>
        internal List<object> Tag { get; set; } = new();

        internal Color ColorLightMode { get; set; } = Color.White;

        internal Color ColorDarkMode { get; set; } = Color.White;

        internal Color ColorMode
        {
            get { return Statics.IsDarkModeActive ? ColorDarkMode : ColorLightMode; }
        }

        /// <summary>
        /// Límites del área donde se puede hacer drag and drop o slide.
        /// </summary>
        internal Rectangle DragDropBounds { get; set; }

        internal bool PlaySoundOnClick { get; set; } = true;

        internal bool EnableOnClick { get; set; } = true;

        internal Vector2 CurrentScale
        {
            get
            {
                return Screen.RedimMatrix * Scale * MasterScale;
            }
        }

        internal float CurrentTransparency
        {
            get
            {
                return Transparency * MasterTransparency;
            }
        }

        /// <summary>
        /// Indica que se ha lanzado el evento click.
        /// </summary>
        internal bool ClickedRaised { get; set; } = false;

        /// <summary>
        /// Si está a true quiere decir que este objeto no sufre el fade-in fade-out de la transición entre ventanas.
        /// </summary>
        internal bool AllowFadeInFadeOut { get; set; } = true;

        /// <summary>
        /// Permite el click cuando el objeto no está visible.
        /// </summary>
        internal bool AllowClickWhenNotVisible { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal InteractiveObject(ModalLevel modalLevel, Rectangle bounds)
            : base(modalLevel, bounds)
        {
            SubscribeEvents();
            IsDarkModeCurrentlyActive = Statics.IsDarkModeActive;
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
                Tag.Clear();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~InteractiveObject()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void OnClick_Raised(object sender, OnClickEventArgs e)
        {
            if (EnableOnClick /*Debe estar habilitado el click*/
                && (Visible || AllowClickWhenNotVisible) /*Debe ser visible o permitir el click cuando no está visible*/
                && !e.ClickEventArgs.DoubleClick /*No debe ser un doble click*/
                && CursorInsideMe(e.ClickEventArgs.Position) /*El cursor debe estar en su superficie*/
                && Screen.IsActive /*La pantalla debe estar activa*/
                && WindowManager.ItsMeTheTopMost(ModalLevel) /*Debe estar en la ventana superior*/
                && Active /*Debe estar activa*/
                && OnClick != null /*Debe haber algo enganchado al manejador*/)
            {
                if (GetType().Name == typeof(Button).Name && PlaySoundOnClick)
                    SoundManager.ButtonClickOpenWindow.PlaySound();

                ClickedRaised = true;
                OnClick?.Invoke(this, new OnClickEventArgs(e.ClickEventArgs.Position));
            }
        }

        void SubscribeEvents()
        {
            TouchManager.OnClick += OnClick_Raised;
        }

        void UnsubscribeEvents()
        {
            TouchManager.OnClick -= OnClick_Raised;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Comprueba si una coordenada está dentro del área de este objeto.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>True, está dentro</returns>
        bool CursorInsideMe(Vector2 position)
        {
            Rectangle bounds = Bounds;

            return IsCircle
                ? Math.Pow(position.X - bounds.X - bounds.Width.Half(), 2) + Math.Pow(position.Y - bounds.Y - bounds.Height.Half(), 2) <= Math.Pow(bounds.Height.Half(), 2)
                : bounds.Contains(position);
        }

        internal override void LoadContent()
        {

        }

        internal override void Update(GameTime gameTime)
        {
            /*Si ha cambiado el modo de color hay que actualizar los elementos de las ventanas activas*/
            if (IsDarkModeCurrentlyActive != Statics.IsDarkModeActive)
            {
                SetColorMode();
                IsDarkModeCurrentlyActive = Statics.IsDarkModeActive;
            }
        }

        internal override void Draw(GameTime gameTime)
        {

        }

        #endregion
    }
}
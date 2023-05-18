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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Link : InteractiveObject, IDisposable
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

        Line UnderlineLine { get; set; }

        /// <summary>
        /// Indica si hay que pintar la clásica linea debajo de un link.
        /// Solo aplica a los InteractiveObjectLink.
        /// </summary>
        internal bool DrawLinkUnderLine { get; private set; } = false;

        Window Window { get; set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor de la clase, bounds debe estar redimensionada, está clase no lo hará.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        internal Link(ModalLevel modalLevel, Rectangle bounds, bool drawLinkUnderLine, Window window)
            : base(modalLevel, bounds)
        {
            Window = window;
            DrawLinkUnderLine = drawLinkUnderLine;
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
        ~Link()
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
            SetLinkUnderline();
        }

        void SetLinkUnderline()
        {
            if (DrawLinkUnderLine)
            {
                UnderlineLine = new Line(ModalLevel, new Point(Bounds.Left, Bounds.Bottom), new Point(Bounds.Right, Bounds.Bottom), ColorManager.LinkLightMode, ColorManager.LinkDarkMode, 1);
                Window.InteractiveObjectManager.Add(UnderlineLine);
            }
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
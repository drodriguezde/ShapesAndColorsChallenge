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

using FontBuddyLib;
using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Label : InteractiveObject, IDisposable
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

        internal Justify Justify { get; set; } = Justify.Left;

        internal string Text { get; set; } = string.Empty;

        AlignHorizontal AlignHorizontal { get; set; }

        /// <summary>
        /// Indica si se debe calcular una vez el ajuste de escala del texto o bien calcularlo a cada Draw.
        /// Para Texto fijo siempre debe ser true, para texto animado en algunos casos deberá se False, como por ejemplo en HeartBeat.
        /// </summary>
        internal bool LockScaleToFit { get; set; } = true;

        /// <summary>
        /// Escala calculada para ajustar el texto a su contenedor.
        /// </summary>
        float ScaletoFit { get; set; } = 0f;

        int LinesNumber { get; set; } = 1;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Este constructor ajustará el contenido a un tipo de fuente prefijado.
        /// Bounds debe estar previamente redimensionado.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="text"></param>
        /// <param name="colorLightMode"></param>
        /// <param name="colorDarkMode"></param>
        internal Label(ModalLevel modalLevel, Rectangle bounds, string text, Color colorLightMode, Color colorDarkMode, AlignHorizontal alignHorizontal = AlignHorizontal.Left, int linesNumber = 1)
            : base(modalLevel, bounds)
        {
            Text = text;
            ColorLightMode = colorLightMode;
            ColorDarkMode = colorDarkMode;
            LinesNumber = linesNumber;
            AlignHorizontal = alignHorizontal;
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
        ~Label()
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
            SetScale();
            SetLines();
        }

        void SetScale()
        {
            Vector2 testSize = new(Bounds.Width * 0.9f * LinesNumber, Bounds.Height / LinesNumber);
            ScaletoFit = FontManager.GetScaleToFit(Text, testSize, LinesNumber);
        }

        void SetLines()
        {
            if (LinesNumber == 1)
                return;

            Text = FontManager.StringInLines(Text, ScaletoFit, Bounds.Width, LinesNumber);
        }

        internal override void Update(GameTime gameTime)
        {
            if (Visible && !LockScaleToFit)
                SetScale();
        }

        internal override void Draw(GameTime gameTime)
        {
            if (Visible)
                FontManager.DrawString(Text, Bounds, ScaletoFit, ColorMode * CurrentTransparency, LinesNumber, AlignHorizontal, Justify);

            base.Draw(gameTime);
        }

        #endregion
    }
}
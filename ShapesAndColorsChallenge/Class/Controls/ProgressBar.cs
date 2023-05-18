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
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class ProgressBar : InteractiveObject, IDisposable
    {
        #region PROPERTIES

        /// <summary>
        /// Valor en el que empieza la barra, puede ser 0 o 1000 o n.
        /// </summary>
        protected long StartValue { get; set; } = 0;

        /// <summary>
        /// Cuanto se ha incrementado StartValue.
        /// </summary>
        protected long Increment { get; set; } = 0;

        /// <summary>
        /// Valor de completado de barra, siempre mayor que StartValue.
        /// </summary>
        protected internal long EndValue { get; private set; } = 0;

        protected bool IsEnded
        {
            get { return Increment >= EndValue; }
        }

        /// <summary>
        /// Cuanto queda para terminar.
        /// </summary>
        protected long ValueToEnd
        {
            get
            {
                return IsEnded ? 0 : EndValue - CurrentValue;
            }
        }

        /// <summary>
        /// Valor en que se encuentra actualmente la barra.
        /// </summary>
        protected long CurrentValue
        {
            get { return StartValue + Increment; }
        }

        /// <summary>
        /// Porcentaje completado del total.
        /// </summary>
        protected double Percent
        {
            get
            {
                return IsEnded ? 100 : 100 * CurrentValue / EndValue;
            }
        }

        /// <summary>
        /// Valor relativo de posición en pantalla.
        /// Se usa para saber dónde empezar a pintar la barra.
        /// </summary>
        protected int RelativeStartX { get; set; } = 0;

        /// <summary>
        /// Valor relativo de posición en pantalla.
        /// Se usa para saber dónde terminar de pintar la barra.
        /// </summary>
        protected int RelativeEndX { get; set; } = 0;

        /// <summary>
        /// Posición de pantalla de hasta dónde llega la barra con el valor actual.
        /// </summary>
        protected int RelativeCurrentValue
        {
            get
            {
                if (IsEnded)
                    return RelativeEndX - RelativeStartX;

                double measure = EndValue - StartValue;
                int measureRelative = RelativeEndX - RelativeStartX;
                int result = (measureRelative * Increment / measure).Ceiling();

                return result > RelativeEndX ? RelativeEndX : result;
            }
        }

        protected Rectangle ProgressBarBounds { get; set; }

        internal Color Color { get; set; } = Color.HotPink;

        /// <summary>
        /// Indica si se debe pintar el progreso en texto.
        /// </summary>
        internal bool DrawProgressString { get; set; } = false;

        internal Color ProgressStringColor { get; set; } = Color.Yellow;

        /// <summary>
        /// Indica si el valor a mostrar es un porcentaje, servirá para mostrar el caracter %.
        /// </summary>
        internal bool IsPercent { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal ProgressBar(ModalLevel modalLevel, long endValue, Rectangle bounds) : base(modalLevel, bounds)
        {
            EndValue = endValue;
            RelativeStartX = Bounds.Left;
            RelativeEndX = Bounds.Right;
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
        ~ProgressBar()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        public void SetValue(long value)
        {
            Increment = value;
        }

        public void Reset()
        {
            Increment = 0;
        }

        internal override void Update(GameTime gameTime)
        {

        }

        internal override void Draw(GameTime gameTime)
        {
            Screen.SpriteBatch.FillRectangle(new Rectangle(Bounds.X, Bounds.Y, RelativeCurrentValue, Bounds.Height), Color);
            Screen.SpriteBatch.DrawRectangle(new Rectangle(RelativeStartX, Bounds.Y, Bounds.Width, Bounds.Height), ColorManager.VeryHardGray, 1f);

            if (DrawProgressString)
                FontManager.DrawString(string.Concat(Increment.ToString(), IsPercent ? "%" : ""), Bounds, 1f, ProgressStringColor, 1, AlignHorizontal.Center);
        }

        #endregion
    }
}
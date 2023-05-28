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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Reflection.Emit;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class PointsPanel : Entity, IDisposable
    {
        #region CONST

        const int TOP = 180;
        const int HEIGHT = 120;
        const int LEFT = 360;
        const int DIGIT_WIDTH = 40;
        const int DIGIT_OFFSET = 3;
        const long MAX_POINTS = 9999999999;

        #endregion

        #region VARS

        Label labelPoints, label01, label02, label03, label04, label05, label06, label07, label08, label09, label10;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Ventana a la que pertenece este Panel.
        /// </summary>
        Window Window { get; set; }

        long Points { get; set; } = default;

        #endregion

        #region CONSTRUCTORS

        internal PointsPanel(ModalLevel modalLevel, Window window) : base(modalLevel)
        { 
            Window = window;
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
        ~PointsPanel()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            SetLabelPoints();
            SetLabelsDigits();
            AddToManager();
        }

        void SetLabelPoints()
        {
            Rectangle bounds = new Rectangle(LEFT, TOP, 240, HEIGHT).Redim();
            labelPoints = new(ModalLevel.Window, bounds, $"{Resource.String.POINTS.GetString()}  "/*dos espacio en blanco*/, ColorManager.HardGray, ColorManager.HardGray);
        }

        void SetLabelsDigits()
        {
            label10 = new(ModalLevel.Window, new(labelPoints.Bounds.Right, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label09 = new(ModalLevel.Window, new(label10.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label08 = new(ModalLevel.Window, new(label09.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label07 = new(ModalLevel.Window, new(label08.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label06 = new(ModalLevel.Window, new(label07.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label05 = new(ModalLevel.Window, new(label06.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label04 = new(ModalLevel.Window, new(label05.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label03 = new(ModalLevel.Window, new(label04.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label02 = new(ModalLevel.Window, new(label03.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
            label01 = new(ModalLevel.Window, new(label02.Bounds.Right + DIGIT_OFFSET, labelPoints.Bounds.Y, DIGIT_WIDTH, labelPoints.Bounds.Height), "0", ColorManager.LightGray, ColorManager.HardGray);
        }

        internal void SetValue(long points)
        {
            if (points > MAX_POINTS)
                Points = MAX_POINTS;
            else
                Points = points;

            string text = Points.ToString().PadLeft(10, '0');

            label01.Text = text.Substring(9, 1);
            label02.Text = text.Substring(8, 1);
            label03.Text = text.Substring(7, 1);
            label04.Text = text.Substring(6, 1);
            label05.Text = text.Substring(5, 1);
            label06.Text = text.Substring(4, 1);
            label07.Text = text.Substring(3, 1);
            label08.Text = text.Substring(2, 1);
            label09.Text = text.Substring(1, 1);
            label10.Text = text.Substring(0, 1);
            label01.ColorLightMode = text == "0000000000" ? ColorManager.LightGray : ColorManager.HardGray;
            label02.ColorLightMode = text[..9] == "000000000" ? ColorManager.LightGray : ColorManager.HardGray;
            label03.ColorLightMode = text[..8] == "00000000" ? ColorManager.LightGray : ColorManager.HardGray;
            label04.ColorLightMode = text[..7] == "0000000" ? ColorManager.LightGray : ColorManager.HardGray;
            label05.ColorLightMode = text[..6] == "000000" ? ColorManager.LightGray : ColorManager.HardGray;
            label06.ColorLightMode = text[..5] == "00000" ? ColorManager.LightGray : ColorManager.HardGray;
            label07.ColorLightMode = text[..4] == "0000" ? ColorManager.LightGray : ColorManager.HardGray;
            label08.ColorLightMode = text[..3] == "000" ? ColorManager.LightGray : ColorManager.HardGray;
            label09.ColorLightMode = text[..2] == "00" ? ColorManager.LightGray : ColorManager.HardGray;
            label10.ColorLightMode = text[..1] == "0" ? ColorManager.LightGray : ColorManager.HardGray;
            label01.ColorDarkMode = text == "0000000000" ? ColorManager.HardGray : ColorManager.LightGray;
            label02.ColorDarkMode = text[..9] == "000000000" ? ColorManager.HardGray : ColorManager.LightGray;
            label03.ColorDarkMode = text[..8] == "00000000" ? ColorManager.HardGray : ColorManager.LightGray;
            label04.ColorDarkMode = text[..7] == "0000000" ? ColorManager.HardGray : ColorManager.LightGray;
            label05.ColorDarkMode = text[..6] == "000000" ? ColorManager.HardGray : ColorManager.LightGray;
            label06.ColorDarkMode = text[..5] == "00000" ? ColorManager.HardGray : ColorManager.LightGray;
            label07.ColorDarkMode = text[..4] == "0000" ? ColorManager.HardGray : ColorManager.LightGray;
            label08.ColorDarkMode = text[..3] == "000" ? ColorManager.HardGray : ColorManager.LightGray;
            label09.ColorDarkMode = text[..2] == "00" ? ColorManager.HardGray : ColorManager.LightGray;
            label10.ColorDarkMode = text[..1] == "0" ? ColorManager.HardGray : ColorManager.LightGray;
        }

        void AddToManager()
        {
            Window.InteractiveObjectManager.Add(labelPoints);
            Window.InteractiveObjectManager.Add(label01, label02, label03, label04, label05, label06, label07, label08, label09, label10);
        }

        internal override void Update(GameTime gameTime)
        {
        }

        internal override void Draw(GameTime gameTime)
        {
        }

        #endregion
    }
}
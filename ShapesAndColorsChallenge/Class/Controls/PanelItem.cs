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
using ShapesAndColorsChallenge.Enum;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class PanelItem : InteractiveObject
    {
        #region PROPERTIES

        /// <summary>
        /// Objectos agregados al item.
        /// </summary>
        internal List<InteractiveObject> InteractiveObjects { get; set; } = new();

        /// <summary>
        /// Indica si este elemento está resaltado.
        /// </summary>
        internal bool Highlight { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        /// <param name="controls">Los bounds de los controles deben ser relativos al Item que lo va a contener.</param>
        internal PanelItem(ModalLevel modalLevel, Rectangle bounds, params InteractiveObject[] controls) : base(modalLevel, bounds)
        {
            InteractiveObjects.AddRange(controls);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        internal void Add(InteractiveObject control)
        {
            InteractiveObjects.Add(control);
        }

        internal void Add(params InteractiveObject[] controls)
        {
            InteractiveObjects.AddRange(controls);
        }

        /// <summary>
        /// Los objetos dentro del item están ubicados de forma relativa al item pero hay que moverlos a una posición absoluta de pantalla.
        /// </summary>
        internal void Move()
        {
            if (!Visible)
                return;

            foreach (InteractiveObject control in InteractiveObjects)
            {
                if (control.GetType().Name == typeof(Line).Name)
                {
                    (control as Line).P1X = Location.X.ToInt() + (control as Line).OriginalP1X;
                    (control as Line).P2X = Location.X.ToInt() + (control as Line).OriginalP2X;
                    (control as Line).P1Y = Location.Y.ToInt() + (control as Line).OriginalP1Y;
                    (control as Line).P2Y = Location.Y.ToInt() + (control as Line).OriginalP2Y;
                }
                else
                    control.Location = Location + control.OriginalLocation;
            }
        }

        internal override void Update(GameTime gameTime)
        {

        }

        internal override void Draw(GameTime gameTime)
        {
            if (Highlight)
                Screen.SpriteBatch.FillRectangle(new(0, Bounds.Top, BaseBounds.Bounds.Width, Bounds.Height), Color.Cyan * CurrentTransparency);
        }

        #endregion
    }
}

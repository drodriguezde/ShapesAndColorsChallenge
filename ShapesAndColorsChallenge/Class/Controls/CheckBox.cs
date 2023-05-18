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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class CheckBox : InteractiveObject, IDisposable
    {
        #region DELEGATES

        internal event EventHandler OnCheckedChange;

        #endregion

        #region VARS

        bool isChecked = false;

        #endregion

        #region PROPERTIES

        Texture2D TextureChecked { get; set; }

        Texture2D TextureUnChecked { get; set; }

        internal bool Checked
        {
            get { return isChecked; }
            private set
            {
                if (value != isChecked)
                {
                    SoundManager.CheckBoxClick.PlaySound();
                    isChecked = value;
                    OnCheckedChange?.Invoke(this, null);
                }
            }
        }

        #endregion

        #region CONSTRUCTORS

        internal CheckBox(ModalLevel modalLevel, Rectangle bounds, bool checkboxChecked)
            : base(modalLevel, bounds)
        {
            isChecked = checkboxChecked;
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
                OnClick -= InteractiveObjectCheckBox_OnClick;
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~CheckBox()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        private void InteractiveObjectCheckBox_OnClick(object sender, EventArgs e)
        {
            Checked = !Checked;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();

            TextureChecked = TextureManager.TextureCheckBoxChecked;
            TextureUnChecked = TextureManager.TextureCheckBoxUnChecked;
            OnClick += InteractiveObjectCheckBox_OnClick;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (Checked)
                Screen.SpriteBatch.Draw(TextureChecked, Bounds, ColorManager.CheckBoxColor * CurrentTransparency);
            else
                Screen.SpriteBatch.Draw(TextureUnChecked, Bounds, ColorManager.CheckBoxColor * CurrentTransparency);

            base.Draw(gameTime);
        }

        #endregion
    }
}
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
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class OptionBox : InteractiveObject, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

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
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    OnCheckedChange?.Invoke(this, null);
                }
            }
        }

        internal List<OptionBox> OptionGroupMembers { get; set; } = new List<OptionBox>();/*No hace falta hacer dispose de estos, ya que están contenidos en InteractiveObjectManager*/

        #endregion

        #region CONSTRUCTORS

        internal OptionBox(ModalLevel modalLevel, Rectangle bounds)
            : base(modalLevel, bounds)
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
                OnClick -= InteractiveObjectOptionBox_OnClick;
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~OptionBox()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        private void InteractiveObjectOptionBox_OnClick(object sender, EventArgs e)
        {
            Checked = true;

            for (int i = 0; i < OptionGroupMembers.Count; i++)
                if (OptionGroupMembers[i].ID != ID)
                    OptionGroupMembers[i].Checked = false;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();

            TextureChecked = TextureManager.TextureOptionBoxChecked;
            TextureUnChecked = TextureManager.TextureOptionBoxUnChecked;
            OnClick += InteractiveObjectOptionBox_OnClick;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (Checked)
                Screen.SpriteBatch.Draw(TextureChecked, Bounds, Color.White * CurrentTransparency);
            else
                Screen.SpriteBatch.Draw(TextureUnChecked, Bounds, Color.White * CurrentTransparency);

            base.Draw(gameTime);
        }

        #endregion
    }
}
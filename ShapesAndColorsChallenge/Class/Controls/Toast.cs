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

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal static class Toast
    {
        #region CONST

        /// <summary>
        /// Tiempo en segundos que será visible.
        /// </summary>
        const float TIME_VISIBLE = 2f;

        #endregion

        #region PROPERTIES

        static Rectangle BodyTextureBounds { get; set; }
        static Rectangle BackTextureBounds { get; set; }
        static bool Running { get; set; } = false;
        static Label Label { get; set; }
        static float Time { get; set; } = 0f;

        #endregion

        #region METHODS

        internal static void Start(string text)
        {
            Running = true;
            Time = 0f;
            Label = new(ModalLevel.None, new(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height - 600, BaseBounds.Limits.Width - 100, 200), text, Color.Black, Color.White, AlignHorizontal.Center, 2);
            Label.LoadContent();
            BodyTextureBounds = new(Label.Bounds.X - 10, Label.Bounds.Y - 10, Label.Bounds.Width + 20, Label.Bounds.Height + 20);
            BackTextureBounds = new(Label.Bounds.X - 20, Label.Bounds.Y - 20, Label.Bounds.Width + 40, Label.Bounds.Height + 40);
        }

        internal static void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            Time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Time >= TIME_VISIBLE)
                Running = false;
        }

        internal static void Draw(GameTime gameTime)
        {
            if (!Running) return;

            Screen.SpriteBatch.FillRectangle(BackTextureBounds, ColorManager.WindowBodyColorInverted);
            Screen.SpriteBatch.FillRectangle(BodyTextureBounds, ColorManager.WindowBodyColor);
            Label.Draw(gameTime);
        }

        #endregion
    }
}
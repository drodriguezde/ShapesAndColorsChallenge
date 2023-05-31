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

using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TouchScreenBuddy;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class DebugManager
    {
        #region VARS

        static FrameRateCounter frameRateCounter = new();

        /// <summary>
        /// Almacena los identificadores de las cadenas de texto de los idiomas.
        /// </summary>
        static List<FieldInfo> listFieldInfoLanguage = new();

        /// <summary>
        /// Cantidad de cadenas de idioma probadas.
        /// </summary>
        static int fieldsTested = 0;

        /// <summary>
        /// Sirva para pintar lso toques y las interacciones del usuario con la pantalla.
        /// </summary>
        static DebugInputComponent debugInputComponent;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Almacena las coordenadas de los toques en pantalla.
        /// </summary>
        static string Coords { get; set; } = string.Empty;

        static TouchComponent TouchComponent
        {
            get
            {
                return TouchManager.TouchComponent;
            }
        }

        /// <summary>
        /// Para que al pulsar sobre la zona de cambio de modo oscuro no lo haga constantemente.
        /// </summary>
        static TimeSpan LastDarkModeChange { get; set; } = TimeSpan.Zero;

        #endregion

        #region METHODS

        internal static void Initialize(Game game)
        {
            /*Pinta en pantalla los toques*/
            debugInputComponent = new DebugInputComponent(game, null)
            {
                Enabled = false
            };
        }

        /// <summary>
        /// Cuando se invoca se prueba el idioma seleccionado actualmente.
        /// </summary>
        public static void RunLanguageTest()
        {
#if ANDROID
            FieldInfo[] fieldInfos = typeof(Resource.String).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
#else
            /*TODO, iOS*/
#endif
            listFieldInfoLanguage = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            fieldsTested = 0;
        }

        static void TestAllLanguageStrings()
        {
            if (fieldsTested == listFieldInfoLanguage.Count)
                return;

            FontManager.DrawString(
                listFieldInfoLanguage[fieldsTested].GetValue(null).ToInt().GetString(),
                new Rectangle(0, 0, BaseBounds.Bounds.Width, (BaseBounds.Bounds.Height * 0.04f).ToInt()),
                1f,
                ColorManager.VersionLightMode * 1f,
                1,
                AlignHorizontal.Center);
            fieldsTested++;
        }

        static void ChangeDarkMode(GameTime gameTime)
        {
            Screen.SpriteBatch.DrawRectangle(new Rectangle(
                (BaseBounds.Bounds.Width - BaseBounds.Bounds.Width * 0.3f).ToInt(),
                (BaseBounds.Bounds.Height - (BaseBounds.Bounds.Height * 0.1f).ToInt()),
                (BaseBounds.Bounds.Width * 0.3f).ToInt(),
                (BaseBounds.Bounds.Height * 0.1f).ToInt()), Color.Orange, 1f);
            Vector2 position = new((BaseBounds.Bounds.Width - BaseBounds.Bounds.Width * 0.3f).ToInt() + 35, (BaseBounds.Bounds.Height - (BaseBounds.Bounds.Height * 0.06f)).ToInt());
            FontManager.GetFont().Write("DARK MODE", position, FontBuddyLib.Justify.Left, 1f, Color.Orange, Screen.SpriteBatch, null);

            foreach (TouchLocation touchLocation in TouchPanel.GetState())
                if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Released || touchLocation.State == TouchLocationState.Moved)
                    if (touchLocation.Position.X > (BaseBounds.Bounds.Width - BaseBounds.Bounds.Width * 0.3f) && touchLocation.Position.Y > BaseBounds.Bounds.Height - (BaseBounds.Bounds.Height * 0.1f))
                    {
                        if (gameTime.TotalGameTime.Subtract(LastDarkModeChange).TotalMilliseconds < 500)
                            return;

                        LastDarkModeChange = gameTime.TotalGameTime;

                        UserSettingsManager.DarkMode = false;
                        UserSettingsManager.AlwaysDarkMode = !UserSettingsManager.AlwaysDarkMode;
                    }
        }

        static void ShowTouch()
        {
            Vector2 position = new(200f, 2050f);

            foreach (TouchLocation touchLocation in TouchPanel.GetState())
                if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Released || touchLocation.State == TouchLocationState.Moved)
                    Coords = $"X: {touchLocation.Position.X} Y: {touchLocation.Position.Y}";

            FontManager.GetFont().Write(Coords, position, FontBuddyLib.Justify.Left, 1f, Color.Green, Screen.SpriteBatch, null);

            position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f);
            FontManager.GetFont().Write(string.Format("Highlights: {0}", TouchComponent.Highlights.Count), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

            position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 50f);
            FontManager.GetFont().Write(string.Format("Clicks: {0}", TouchComponent.Clicks.Count), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

            position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 100f);
            FontManager.GetFont().Write(string.Format("Drags: {0}", TouchComponent.Drags.Count), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

            position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 150f);
            FontManager.GetFont().Write(string.Format("Drops: {0}", TouchComponent.Drops.Count), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

            position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 200f);
            FontManager.GetFont().Write(string.Format("Holds: {0}", TouchComponent.Holds.Count), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

            if (TouchComponent.Pinches.Count > 0)/*Dos o más dedos a la vez*/
            {
                var pinch = TouchComponent.Pinches.First();
                position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 250f);
                FontManager.GetFont().Write(string.Format("Pinch Delta: {0}", pinch.Delta.ToString()), position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);

                position = new Vector2(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Height * 0.04f + 300f);
                FontManager.GetFont().Write(pinch.Delta < 0f ? "Zoom out" : "Zoom in", position, FontBuddyLib.Justify.Left, 1f, ColorManager.LinkDarkMode, Screen.SpriteBatch, null);
            }
        }

        static void ShowLimitLines()
        {
            /*Linea vertical*/
            Screen.SpriteBatch.DrawLine(BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Top, BaseBounds.Bounds.Width.Half(), BaseBounds.Bounds.Bottom, ColorManager.VersionColor, 1f, 1);
            /*Linean horizontal*/
            Screen.SpriteBatch.DrawLine(BaseBounds.Bounds.Left, BaseBounds.Bounds.Height.Half(), BaseBounds.Bounds.Right, BaseBounds.Bounds.Height.Half(), ColorManager.VersionColor, 1f, 1);
            /*Área*/
            Screen.SpriteBatch.DrawRectangle(new Rectangle(
                (BaseBounds.Bounds.X + BaseBounds.Bounds.Width * 0.025f).ToInt(),
                (BaseBounds.Bounds.Y + BaseBounds.Bounds.Width * 0.05f).ToInt(),
                (BaseBounds.Bounds.Width - BaseBounds.Bounds.Width * 0.05f).ToInt(),
                (BaseBounds.Bounds.Height - BaseBounds.Bounds.Width * 0.1f).ToInt()), ColorManager.LinkDarkMode, 1);
        }

        static void ShowWindows()
        {
            /*Windows*/
            Vector2 position = new(50f, BaseBounds.Bounds.Height * 0.03f + 260f);
            FontManager.GetFont().Write(string.Format("Windows: {0}", WindowManager.Windows.Count), position, FontBuddyLib.Justify.Left, 1f, Color.OrangeRed, Screen.SpriteBatch, null);
        }

        static void ShowInteractiveObjects()
        {
            for (int i = 0; i < WindowManager.Windows.Count; i++)
            {
                Vector2 position = new(BaseBounds.Bounds.Width.Half() - BaseBounds.Bounds.Width.Half() * 0.2f, 40);
                FontManager.GetFont().Write(WindowManager.Windows[i].WindowType.ToString(), position, FontBuddyLib.Justify.Left, 1f, Color.Orange, Screen.SpriteBatch, null);

                foreach (InteractiveObject interactiveObject in WindowManager.Windows[i].InteractiveObjectManager.InteractiveObjects)
                    if (interactiveObject.Visible)
                        if (interactiveObject.GetType().Name != typeof(Line).Name)
                            Screen.SpriteBatch.DrawRectangle(interactiveObject.Bounds, ColorManager.VersionColor, 1f);
            }
        }

        internal static void Update(GameTime gameTime)
        {
            frameRateCounter.Update(gameTime);
        }

        internal static void Draw(GameTime gameTime, bool showTextLanguage, bool showFPS, bool showTouch, bool showLimitLines, bool showWindows, bool showInteractiveObjects, bool showDarkMode)
        {
            if (showTextLanguage)
                TestAllLanguageStrings();

            if (showFPS)
                frameRateCounter.DrawFps(new Vector2(40f, 40f), ColorManager.VersionColor);

            if (showTouch)
            {
                debugInputComponent.Enabled = showTouch;
                ShowTouch();
            }

            if (showLimitLines)
                ShowLimitLines();

            if (showWindows)
                ShowWindows();

            if (showInteractiveObjects)
                ShowInteractiveObjects();

            if (showDarkMode)
                ChangeDarkMode(gameTime);
        }

        #endregion
    }
}
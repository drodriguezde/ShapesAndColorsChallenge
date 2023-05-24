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
* DISPOSE CONTROL : STATIC
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
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class FontManager
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        //GameClock clock = new GameClock();

        #endregion

        #region PROPERTIES

        internal static FontBuddyPlus Noto { get; private set; } = null;
        internal static FontBuddyPlus NotoJP { get; private set; } = null;
        internal static FontBuddyPlus NotoSC { get; private set; } = null;
        internal static FontBuddyPlus NotoKR { get; private set; } = null;

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        /// <summary>
        /// Las fuentas NOTO tardan en cargar así que se carga la configurada y si el usuario cambia de idioma se carga esa en ese momento.
        /// </summary>
        internal static void LoadContent()
        {
            GetFont();
        }

        static FontBuddyPlus LoadRegular()
        {
            Noto ??= new FontBuddyPlusStroked();
            Noto.LoadContent(GameContent.ContentFont, @"Font/NotoSans-Regular", true, 60);
            return Noto;
        }

        static FontBuddyPlus LoadJP()
        {
            NotoJP ??= new FontBuddyPlusStroked();
            NotoJP.LoadContent(GameContent.ContentFont, @"Font/NotoSansJP-Regular", true, 60);
            return NotoJP;
        }

        static FontBuddyPlus LoadSC()
        {
            NotoSC ??= new FontBuddyPlusStroked();
            NotoSC.LoadContent(GameContent.ContentFont, @"Font/NotoSansSC-Regular", true, 60);
            return NotoSC;
        }

        static FontBuddyPlus LoadKR()
        {
            NotoKR ??= new FontBuddyPlusStroked();
            NotoKR.LoadContent(GameContent.ContentFont, @"Font/NotoSansKR-Regular", true, 60);
            return NotoKR;
        }

        internal static FontBuddyPlus GetFont()
        {
            return UserSettingsManager.CountryCode switch
            {
                "zh" => LoadSC(),
                "ja" => LoadJP(),
                "ko" => LoadKR(),
                _ => LoadRegular()
            };
        }

        /// <summary>
        /// Devuelve true si la fuente en uso tiene separadores de espacios.
        /// </summary>
        /// <returns></returns>
        internal static bool FontWithSpaces()
        {
            return UserSettingsManager.CountryCode switch
            {
                "zh" or "ja" => false,
                _ => true,
            };
        }

        internal static void DrawString(string text, Rectangle bounds, float scale, Color color, int linesNumber, AlignHorizontal alignHorizontal = AlignHorizontal.Left, Justify justify = Justify.Left)
        {
            Vector2 stringSize = GetFont().MeasureString(text) * new Vector2(scale, scale);
            Vector2 position = Vector2.One;
            float yCorrection = linesNumber * stringSize.Y.Half();

            switch (alignHorizontal)
            {
                case AlignHorizontal.Center:
                    position = new Vector2(bounds.X + bounds.Width.Half() - stringSize.X.Half(), bounds.Y + bounds.Height.Half() - yCorrection);
                    break;
                case AlignHorizontal.Left:
                    position = new Vector2(bounds.X, bounds.Y + bounds.Height.Half() - yCorrection);
                    break;
                case AlignHorizontal.Right:
                    position = new Vector2(bounds.Right - stringSize.X, bounds.Y + bounds.Height.Half() - yCorrection);
                    break;
            }

            GetFont().Write(text, position, justify, scale, color, Screen.SpriteBatch, null);
        }

        internal static float GetScaleToFit(string text, Vector2 scaleSize, int linesNumber = 1)
        {
            FontBuddyPlus font = GetFont();
            float scaleToFit = 1f;
            Vector2 stringSize = font.MeasureString(string.Concat(text, new string(' ', linesNumber - 1))) * new Vector2(scaleToFit, scaleToFit);

            if (stringSize.Y > scaleSize.Y)/*Nos hemos pasado, hay que reducir*/
                while (stringSize.Y > scaleSize.Y)/*Vamos reduciendo*/
                {
                    scaleToFit -= 0.01f;
                    stringSize = font.MeasureString(text) * new Vector2(scaleToFit, scaleToFit);
                }

            if (stringSize.Y < scaleSize.Y)/*No hemos llegado, hay que aumentar*/
                while (stringSize.Y < scaleSize.Y)/*Vamos aumentando*/
                {
                    scaleToFit += 0.01f;
                    stringSize = font.MeasureString(text) * new Vector2(scaleToFit, scaleToFit);
                }

            if (stringSize.X > scaleSize.X)/*Nos hemos pasado, hay que reducir*/
                while (stringSize.X > scaleSize.X)/*Vamos reduciendo*/
                {
                    scaleToFit -= 0.01f;
                    stringSize = font.MeasureString(text) * new Vector2(scaleToFit, scaleToFit);
                }

            /*Este bloque no se usa o se deforma el ratio de aspecto para los textos*/
            //if (stringSize.X < bounds.Width)/*No hemos llegado, hay que aumentar*/
            //    while (stringSize.X < bounds.Width)/*Vamos aumentando*/
            //    {
            //        scaleToFit += 0.01f;
            //        stringSize = font.MeasureString(text) * new Vector2(scaleToFit, scaleToFit);
            //    }

            return scaleToFit;
        }

        /// <summary>
        /// Devuelve una cadena de texto en varias lineas.
        /// </summary>
        /// <returns></returns>
        internal static string StringInLines(string text, float scaleToFit, float width, int linesNumber)
        {
            List<string> lines = new() { "" };

            if (FontWithSpaces())
            {
                List<string> words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string word in words)
                {
                    if (GetFont().MeasureString(string.Concat(lines.Last().Trim(), " ", word)).X * scaleToFit < width)/*No se llenará la línea con esta palabra*/
                        lines[lines.Count - 1] = string.Concat(lines.Last(), word, " ");
                    else
                    {
                        lines[lines.Count - 1] = lines[lines.Count - 1].Trim();
                        lines.Add(string.Concat(word, " "));
                    }
                }
            }
            else
            {
                List<char> letters = text.ToCharArray().ToList();

                foreach (char letter in letters)
                {
                    if (GetFont().MeasureString(string.Concat(lines.Last(), letter)).X * scaleToFit < width)
                        lines[lines.Count - 1] = string.Concat(lines.Last(), letter);
                    else
                        lines.Add(letter.ToString());
                }
            }

            if (lines.Count > linesNumber)
            {
                lines[lines.Count - 2] = string.Concat(lines[lines.Count - 2], FontWithSpaces() ? " " : "", lines[lines.Count - 1]);
                lines.RemoveAt(lines.Count - 1);
            }

            return string.Join("\n", lines);
        }

        /// <summary>
        /// Centra un texto dentro de un rectangulo.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="textSize"></param>
        /// <returns></returns>
        internal static Vector2 CenterText(this Rectangle container, Vector2 textSize)
        {
            return new Vector2(
                container.X + container.Width.Half() - textSize.X.Half(),
                container.Y + container.Height.Half() - textSize.Y.Half());
        }

        internal static string AccommodateTextWidth(this SpriteFont font, string text, int maxWith)
        {
            string accommodate = string.Empty;
            List<string> words = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < words.Count; i++)
            {
                if (font.MeasureString(string.Concat(accommodate, " ", words[i])).X < maxWith)
                    accommodate = string.Concat(accommodate, i == 0 ? "" : " ", words[i]);
                else
                    accommodate = string.Concat(accommodate, "\r\n", words[i]);
            }

            return accommodate;
        }

        internal static int GetBaselineSpace(this SpriteFont font)
        {
            int med = font.Glyphs.Average(t => t.BoundsInTexture.Height).ToInt();
            med = font.Glyphs.Where(t => t.BoundsInTexture.Height >= med).Average(t => t.BoundsInTexture.Height).ToInt();
            return font.LineSpacing - med;
        }

        /// <summary>
        /// Devuelve la escala que debe tener una fuente para ajustarse a un contenedor.
        /// </summary>
        /// <returns></returns>
        internal static Vector2 Fit(string text, Rectangle bounds, SpriteFont font)
        {
            Vector2 size = font.MeasureString(text);

            if (size.X > bounds.Width || size.Y > bounds.Height)
                return FitDecrease(size, bounds);
            else
                return FitIncrease(size, bounds);
        }

        /// <summary>
        /// Devuelve la escala que debe tener una fuente para ajustarse a un contenedor.
        /// </summary>
        /// <returns></returns>
        internal static Vector2 Fit(string text, Rectangle bounds, FontBuddyPlus font)
        {
            Vector2 size = font.MeasureString(text);

            if (size.X > bounds.Width || size.Y > bounds.Height)
                return FitDecrease(size, bounds);
            else
                return FitIncrease(size, bounds);
        }

        /// <summary>
        /// Incrementa la escala de la fuente.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        static Vector2 FitIncrease(Vector2 size, Rectangle bounds)
        {
            float scaleIncrement = 0.01f;
            Vector2 scale = Vector2.One;

            while (true)
            {
                scale = new Vector2(scale.X + scaleIncrement, scale.Y + scaleIncrement);

                if ((size.X * scale.X) > bounds.Width || (size.Y * scale.Y) > bounds.Height)
                    return new Vector2(scale.X - scaleIncrement, scale.Y - scaleIncrement);

            }
        }

        /// <summary>
        /// Reduce la escala de la fuente.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        static Vector2 FitDecrease(Vector2 size, Rectangle bounds)
        {
            float scaleDecrement = 0.01f;
            Vector2 scale = Vector2.One;

            while (true)
            {
                scale = new Vector2(scale.X - scaleDecrement, scale.Y - scaleDecrement);

                if ((size.X * scale.X) < bounds.Width && (size.Y * scale.Y) < bounds.Height)
                    return scale;
            }
        }

        #endregion
    }
}
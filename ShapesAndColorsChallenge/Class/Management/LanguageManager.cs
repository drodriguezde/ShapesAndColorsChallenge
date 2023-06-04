using Android.App;
using Android.Content;
using Android.Content.Res;
using Java.Util;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class LanguageManager
    {
        #region PROPERTIES

        static Dictionary<string, string> Dictionary { get; set; }

        static string Language { get; set; }

        static CultureInfo CultureInfo { get; set; }

        #endregion

        #region METHODS

        internal static void SetLanguage(string language)
        {
            Language = language;
            CultureInfo = new CultureInfo(language);
            Dictionary = new Dictionary<string, string>();
            Context appContext = Application.Context;
            AssetManager assets = appContext.Assets;

            using var stream = assets.Open($"Values-{language}/Strings.xml");
            using var reader = XmlReader.Create(stream);
            try
            {
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("string"))
                    {
                        string key = reader.GetAttribute("name");
                        string value = reader.ReadInnerXml();
                        Dictionary[key] = value;
                    }
            }
            catch
            {

            }
        }

        internal static string Get(string key)
        {
            try
            {
                return Dictionary[key];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="key">Identificador único de la cadena</param>
        /// <param name="param">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string Get(string key, string param)
        {
            return Dictionary[key].Replace("%1$s", param);
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="key">Identificador único de la cadena</param>
        /// <param name="param">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string Get(string key, int param)
        {
            return Dictionary[key].Replace("%1$d", param.ToString());
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="key">Identificador único de la cadena</param>
        /// <param name="param1">Parámetro que se sustituirá</param>
        /// <param name="param2">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string Get(string key, int param1, int param2)
        {
            return Dictionary[key].Replace("%1$d", param1.ToString()).Replace("%2$d", param2.ToString());
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="key">Identificador único de la cadena</param>
        /// <param name="param1">Parámetro que se sustituirá</param>
        /// <param name="param2">Parámetro que se sustituirá</param>
        /// <param name="param3">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string Get(string key, int param1, int param2, int param3)
        {
            return Dictionary[key].Replace("%1$d", param1.ToString()).Replace("%2$d", param2.ToString()).Replace("%3$d", param3.ToString());
        }

        /// <summary>
        /// Obtiene el lenguaje del sistema operativo.
        /// </summary>
        /// <returns></returns>
        internal static string GetSystemLanguage()
        {
#if ANDROID
            return GetTranslation(Locale.Default.Country.ToLower());
#else
            return NSLocale.CurrentLocale.LocaleIdentifier;
#endif
        }

        internal static CultureInfo GetCultureInfo()
        {
            try
            {
                CultureInfo ??= new CultureInfo(Locale.Default.Language);
            }
            catch
            {
                CultureInfo = new CultureInfo("en");
            }

            return CultureInfo;
        }

        /// <summary>
        /// Comprueba si el idioma actual del dispositivo está entre los posibles de la aplicación.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns>Devuelve un códivo válido, de existir traducción, de no existir devuelve "gb"</returns>
        static string GetTranslation(string countryCode)
        {
            return countryCode switch
            {
                "cs" or "da" or "de" or "es" or "fi" or "fr" or "en" or "hu" or "it" or "ja" or "ko" or "nl" or "no" or "pl" or "pt" or "ru" or "sv" or "tr" or "zh" => countryCode,
                _ => "en",
            };
        }

        #endregion
    }
}

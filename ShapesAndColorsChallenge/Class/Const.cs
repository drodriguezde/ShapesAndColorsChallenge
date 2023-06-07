using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class
{
    internal class Const
    {
        #region CONST

        /// <summary>
        /// Cantidad de tiempo (milisegundos) que la ventana con el logotipo de Dan Studios está visible.
        /// Tiene sumado 500 de Fade-In.
        /// </summary>
        internal const int DAN_STUDIOS_WINDOW_SHOW_TIME = 2000;

        /// <summary>
        /// Cantidad de tiempo (milisegundos) entre click al botón back.
        /// </summary>
        internal const int TIME_BETWEEN_BACK_BUTTON_CLICK = 600;

        /// <summary>
        /// Ancho de la barra de deslizamiento horizontal para el control SlideBar.
        /// </summary>
        internal const int SLIDE_BAR_THICK = 4;

#if ANDROID
        /// <summary>
        /// Dirección de la página de la tienda de Android.
        /// </summary>
        internal const string DAN_SITE_URL = "market://search?q=pub:DanStudios";
#else
/*TODO, dirección de la página para iOS*/


#endif

        /// <summary>
        /// Borde de los botones, cuando se les pone alrededor un rectangulo.
        /// </summary>
        internal const int BUTTON_BORDER = 4;

        /// <summary>
        /// Tasa de frames por segundo.
        /// </summary>
        internal const int GAME_FRAME_RATE = 60;

        /// <summary>
        /// Color del fondo de las ventanas emergentes.
        /// </summary>
        internal const float WINDOW_INNER_TRANSPARENCY = 1f;

        /// <summary>
        /// Nombre que tendrá el jugador cuando inicie por primera vez.
        /// </summary>
        internal const string PLAYER_NAME = "PLAYER";

        /// <summary>
        /// Frames en GAME_FRAME_RATE que necesitan la animaciones para completarse (60fps).
        /// </summary>
        internal const int ANIMATED_STAR_LOOP = 30;
        internal const int ANIMATED_PERK_CHANGE = 120;
        internal const int ANIMATED_PERK_REVEAL = 120;
        internal const int ANIMATED_PERK_TIME_STOP = 90;

        /// <summary>
        /// Diametro de las burbujas de información.
        /// </summary>
        internal const int BUBBLE_INFO_DIAMETER = 80;

        /// <summary>
        /// Padding de la imagen de la ficha con respecto a su contenedor.
        /// </summary>
        internal const int TILE_PADDING = 10;

        internal static readonly List<string> ALL_FLAGS = new() {
            "af",
            "ag",
            "al",
            "am",
            "an",
            "ao",
            "ar",
            "as",
            "au",
            "ba",
            "bb",
            "bc",
            "be",
            "bl",
            "bn",
            "bo",
            "br",
            "bt",
            "bu",
            "by",
            "ca",
            "cd",
            "cg",
            "ci",
            "cm",
            "zh",/*cn*/
            "co",
            "css",
            "ct",
            "cu",
            "cv",
            "cy",
            "cs",/*cz, checoslovaquia, para casar con el lenguaje*/
            "de",
            "dj",
            "da",/*dk, dinamarca, para casar con el lenguaje*/
            "dr",
            "ec",
            "eg",
            "ei",
            "ek",
            "enn",
            "es",
            "et",
            "fi",
            "fr",
            "ga",
            "en",/*gb*/
            "gg",
            "gh",
            "gm",
            "gr",
            "gt",
            "gv",
            "gy",
            "ha",
            "ho",
            "hr",
            "hu",
            "ic",
            "id",
            "in",
            "ir",
            "is",
            "it",
            "iv",
            "iz",
            "jm",
            "jo",
            "ja",/*jp*/
            "ke",
            "kg",
            "kn",
            "ko",/*kr*/
            "ku",
            "kz",
            "le",
            "lg",
            "lh",
            "li",
            "lo",
            "ls",
            "lt",
            "lu",
            "ly",
            "ma",
            "md",
            "mi",
            "mj",
            "mk",
            "ml",
            "mn",
            "mo",
            "mp",
            "mr",
            "mt",
            "mv",
            "mx",
            "ng",
            "ni",
            "nl",
            "no",
            "ns",
            "nu",
            "nz",
            "od",
            "pa",
            "pe",
            "pk",
            "pl",
            "pm",
            "pp",
            "ps",
            "pt",
            "pu",
            "qa",
            "ri",
            "ro",
            "rp",
            "ru",
            "rw",
            "sa",
            "sv",/*se*/
            "sf",
            "sg",
            "si",
            "sl",
            "sm",
            "sn",
            "so",
            "st",
            "su",
            "svv",
            "sy",
            "sz",
            "td",
            "th",
            "ti",
            "tn",
            "to",
            "tr",
            "ts",
            "tt",
            "tx",
            "tz",
            "ug",
            "up",
            "us",
            "uv",
            "uy",
            "uz",
            "ve",
            "vm",
            "wa",
            "ws",
            "wz",
            "ym",
            "za",
            "zi" };

        #endregion
    }
}
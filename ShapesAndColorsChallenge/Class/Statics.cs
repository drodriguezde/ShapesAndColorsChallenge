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

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Controls;
using Android.Content.Res;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System.Globalization;
using Android.Widget;
using AndroidApp = Android.App;
using Microsoft.Xna.Framework.Graphics;
using Android.Net;
using System.Net.Http;
using Android.Content;
using System.Threading.Tasks;
#if ANDROID
using Java.Util;
#else
/*https://docs.microsoft.com/es-es/dotnet/api/Foundation.NSLocale?view=xamarin-ios-sdk-12*/
#endif

namespace ShapesAndColorsChallenge.Class
{
    internal static class Statics
    {
        #region VARS

        static long id = 0;
        static CultureInfo currentCulture = null;

        #endregion

        #region PROPERTIES

        internal static bool IsDarkModeActive
        {
            get
            {
                return UserSettingsManager.AlwaysDarkMode || (UserSettingsManager.DarkMode && (DateTime.Now.Hour >= 20 || DateTime.Now.Hour <= 8));
            }
        }

        #endregion

        #region METHODS

        internal static void TraceException(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            //Debug.WriteLine("date: " + DateTime.Now);
            //Debug.WriteLine("message: " + message);
            //Debug.WriteLine("member name: " + memberName);
            //Debug.WriteLine("source file path: " + sourceFilePath);
            //Debug.WriteLine("source line number: " + sourceLineNumber);
        }

        /// <summary>
        /// Devuelve el punto de intersección donde se cuzan dos lineas rectas
        /// Donde se cruza la recta a con la recta b
        /// </summary>
        /// <returns>Devuelve el punto de intersección de la recta a con la recta b</returns>
        internal static Point FindIntersectionPointBetweenTwoLines(Line line1, Line line2)
        {
            return FindIntersectionPointBetweenTwoLines(line1.Point1.ToVector2(), line1.Point2.ToVector2(), line2.Point1.ToVector2(), line2.Point2.ToVector2()).ToPoint();
        }

        /// <summary>
        /// Devuelve el punto de intersección donde se cuzan dos lineas rectas
        /// Donde se cruza la recta a con la recta b
        /// </summary>
        /// <param name="point1Line1">Es el punto xy de uno de los extremos de la recta a</param>
        /// <param name="point2Line1">Es el punto xy del otro extremo de la recta a</param>
        /// <param name="point1Line2">Es el punto xy de uno de los extremos de la recta b</param>
        /// <param name="point2Line2">Es el punto xy del otro extremo de la recta a</param>
        /// <returns>Devuelve el punto de intersección de la recta a con la recta b</returns>
        internal static Vector2 FindIntersectionPointBetweenTwoLines(Vector2 point1Line1, Vector2 point2Line1, Vector2 point1Line2, Vector2 point2Line2)
        {
            float a1 = point2Line1.Y - point1Line1.Y;
            float b1 = point1Line1.X - point2Line1.X;
            float c1 = a1 * point1Line1.X + b1 * point1Line1.Y;

            float a2 = point2Line2.Y - point1Line2.Y;
            float b2 = point1Line2.X - point2Line2.X;
            float c2 = a2 * point1Line2.X + b2 * point1Line2.Y;

            float delta = a1 * b2 - a2 * b1;

            //Si las líneas son paralelas, el resultado será (NaN,NaN)
            return delta == 0 ? new(float.NaN, float.NaN) : new((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
        }

        /// <summary>
        /// Obtiene un número entero aleatorio comprendido entre el valor mínimo y el máximo, ambos inclusive.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        internal static int GetRandom(int minValue, int maxValue)
        {
            byte[] buffer = new byte[4];

            using var random = RandomNumberGenerator.Create();
            random.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);

            return new System.Random(result).Next(minValue, maxValue + 1);
        }

        internal static string GetUserPath()
        {
            return string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"\", AppDomain.CurrentDomain.FriendlyName.ToLower().Replace(".exe", ""), @"\");
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
                if (currentCulture == null)
                    currentCulture = new CultureInfo(Locale.Default.Language);
            }
            catch
            {
                currentCulture = new CultureInfo("en");
            }

            return currentCulture;
        }

        /// <summary>
        /// Comprueba si el idioma actual del dispositivo está entre los posibles de la aplicación.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns>Devuelve un códivo válido, de existir traducción, de no existir devuelve "gb"</returns>
        static string GetTranslation(string countryCode)
        {
            switch (countryCode)
            {
                case "cs":
                case "da":
                case "de":
                case "es":
                case "fi":
                case "fr":
                case "en":
                case "hu":
                case "it":
                case "ja":
                case "ko":
                case "nl":
                case "no":
                case "pl":
                case "pt":
                case "ru":
                case "sv":
                case "tr":
                case "zh":
                    return countryCode;
                default:
                    return "en";
            }
        }

        internal static string GetGameModeTitle()
        {
            return OrchestratorManager.GameMode switch
            {
                GameMode.Blink => Resource.String.BLINK_MODE.GetString(),
                GameMode.BlinkPlus => Resource.String.BLINK_MODE_PLUS.GetString(),
                GameMode.Rotate => Resource.String.ROTATE_MODE.GetString(),
                GameMode.RotatePlus => Resource.String.ROTATE_MODE_PLUS.GetString(),
                GameMode.Classic => Resource.String.CLASSIC_MODE.GetString(),
                GameMode.ClassicPlus => Resource.String.CLASSIC_MODE_PLUS.GetString(),
                GameMode.Endless => Resource.String.ENDLESS_MODE.GetString(),
                GameMode.EndlessPlus => Resource.String.ENDLESS_MODE_PLUS.GetString(),
                GameMode.Incremental => Resource.String.INCREMENTAL_MODE.GetString(),
                GameMode.IncrementalPlus => Resource.String.INCREMENTAL_MODE_PLUS.GetString(),
                GameMode.Memory => Resource.String.MEMORY_MODE.GetString(),
                GameMode.MemoryPlus => Resource.String.MEMORY_MODE_PLUS.GetString(),
                GameMode.Move => Resource.String.MOVE_MODE.GetString(),
                GameMode.MovePlus => Resource.String.MOVE_MODE_PLUS.GetString(),
                GameMode.TimeTrial => Resource.String.TIMETRIAL_MODE.GetString(),
                GameMode.TimeTrialPlus => Resource.String.TIMETRIAL_MODE_PLUS.GetString(),
                _ => string.Empty,
            };
        }

        internal static string GetHowToPlayDescription(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.Classic => Resource.String.HTP_CLASSIC_MODE.GetString(),
                GameMode.Incremental => Resource.String.HTP_INCREMENTAL_MODE.GetString(),
                GameMode.Endless => Resource.String.HTP_ENDLESS_MODE.GetString(),
                GameMode.Move => Resource.String.HTP_MOVE_MODE.GetString(),
                GameMode.Memory => Resource.String.HTP_MEMORY_MODE.GetString(),
                GameMode.Blink => Resource.String.HTP_BLINK_MODE.GetString(),
                GameMode.TimeTrial => Resource.String.HTP_TIMETRIAL_MODE.GetString(),
                GameMode.Rotate => Resource.String.HTP_ROTATE_MODE.GetString(),
                GameMode.ClassicPlus => Resource.String.HTP_CLASSIC_MODE_PLUS.GetString(),
                GameMode.IncrementalPlus => Resource.String.HTP_INCREMENTAL_MODE_PLUS.GetString(),
                GameMode.EndlessPlus => Resource.String.HTP_ENDLESS_MODE_PLUS.GetString(),
                GameMode.MovePlus => Resource.String.HTP_MOVE_MODE_PLUS.GetString(),
                GameMode.MemoryPlus => Resource.String.HTP_MEMORY_MODE_PLUS.GetString(),
                GameMode.BlinkPlus => Resource.String.HTP_BLINK_MODE_PLUS.GetString(),
                GameMode.TimeTrialPlus => Resource.String.HTP_TIMETRIAL_MODE_PLUS.GetString(),
                GameMode.RotatePlus => Resource.String.HTP_ROTATE_MODE_PLUS.GetString(),
                _ => null,
            };
        }

        internal static Texture2D GetHowToPlayTexture(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.Classic => TextureManager.GetTexture("Image/HTPClassic"),
                GameMode.Incremental => TextureManager.GetTexture("Image/HTPIncremental"),
                GameMode.Endless => TextureManager.GetTexture("Image/HTPEndless"),
                GameMode.Move => TextureManager.GetTexture("Image/HTPMove"),
                GameMode.Memory => TextureManager.GetTexture("Image/HTPMemory"),
                GameMode.Blink => TextureManager.GetTexture("Image/HTPBlink"),
                GameMode.TimeTrial => TextureManager.GetTexture("Image/HTPTimeTrial"),
                GameMode.Rotate => TextureManager.GetTexture("Image/HTPRotate"),
                GameMode.ClassicPlus => TextureManager.GetTexture("Image/HTPClassicPlus"),
                GameMode.IncrementalPlus => TextureManager.GetTexture("Image/HTPIncrementalPlus"),
                GameMode.EndlessPlus => TextureManager.GetTexture("Image/HTPEndlessPlus"),
                GameMode.MovePlus => TextureManager.GetTexture("Image/HTPMovePlus"),
                GameMode.MemoryPlus => TextureManager.GetTexture("Image/HTPMemoryPlus"),
                GameMode.BlinkPlus => TextureManager.GetTexture("Image/HTPBlinkPlus"),
                GameMode.TimeTrialPlus => TextureManager.GetTexture("Image/HTPTimeTrialPlus"),
                GameMode.RotatePlus => TextureManager.GetTexture("Image/HTPRotatePlus"),
                _ => null,
            };
        }

        internal static void SetLocale(string languageCode)
        {
#if ANDROID
            Locale locale = new Locale(languageCode);
            Locale.SetDefault(Locale.Category.Display, locale);
            Resources resources = Android.App.Application.Context.Resources;
            Configuration config = resources.Configuration;
            config.SetLocale(locale);
            //Application.Context.ApplicationContext.CreateConfigurationContext(config);
            //Application.Context.Resources.DisplayMetrics.SetTo(resources.DisplayMetrics);
            resources.UpdateConfiguration(config, resources.DisplayMetrics);
#else
            /*TODO, la parte de iOS*/
#endif
        }

        internal static string GetAppVersion()
        {
#if ANDROID
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
            
#endif
        }

        /// <summary>
        /// Comprueba si hay coneción a internet
        /// </summary>
        /// <returns></returns>
        internal static bool CheckConectivity()
        {
#if ANDROID
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);

            // Comprueba si hay una conexión activa
            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            bool isConnected = (activeNetworkInfo != null) && activeNetworkInfo.IsConnected;

            // Verifica si hay una conexión a Internet
            bool hasInternetConnection = isConnected/* && CheckInternetConnectivity().GetAwaiter().GetResult()*/;

            return hasInternetConnection;
#else
            
#endif
        }

        static async Task<bool> CheckInternetConnectivity()
        {
            try
            {
                using HttpClient client = new();
                using HttpResponseMessage response = await client.GetAsync("https://www.google.com");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        internal static long NewID()
        {
            return id++;
        }

        internal static float GetAngleLines(Vector2 v1, Vector2 v2)
        {
            return GetAngleLines(v1.X.ToInt(), v2.X.ToInt(), v1.Y.ToInt(), v2.Y.ToInt());
        }

        internal static float GetAngleLines(int p1X, int p2X, int p1Y, int p2Y)
        {
            return Math.Atan2(p2Y - p1Y, p2X - p1X).ToSingle();
        }

        internal static int GetDistance(int p1X, int p2X, int p1Y, int p2Y)
        {
            return Math.Sqrt(Math.Pow(p1X - p2X, 2) + Math.Pow(p1Y - p2Y, 2)).ToInt();
        }

        /// <summary>
        /// Hace vibrar el dispositivo los milisegundos indicados.
        /// </summary>
        /// <param name="miliseconds"></param>
        internal static void Vibrate(int miliseconds)
        {
            var vibrator = (Android.OS.Vibrator)AndroidApp.Application.Context.GetSystemService(Android.Content.Context.VibratorService);

            if (vibrator.HasVibrator && UserSettingsManager.Vibration)
                vibrator.Vibrate(miliseconds);
        }

        /// <summary>
        /// Hace vibrar el dispositivo un tiempo determinado.
        /// </summary>
        /// <param name="vibrationDuration"></param>
        internal static void Vibrate(VibrationDuration vibrationDuration)
        {
            Vibrate(vibrationDuration.ToInt());
        }

        /// <summary>
        /// Muestra un mensaje toast.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="length"></param>
        internal static void ShowToast(string message, ToastLength length)
        {
            Toast.MakeText(AndroidApp.Application.Context, message, length).Show();
        }

        #endregion
    }
}
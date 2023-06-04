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

using Android.Content;
using Android.Net;
using Android.OS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using AndroidApp = Android.App;
#if ANDROID
#else
/*https://docs.microsoft.com/es-es/dotnet/api/Foundation.NSLocale?view=xamarin-ios-sdk-12*/
#endif

namespace ShapesAndColorsChallenge.Class
{
    internal static class Statics
    {
        #region VARS

        static long id = 0;

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
        /// Devuelve el punto de intersección donde se cruzan dos lineas rectas
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
            return string.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), @"\", AppDomain.CurrentDomain.FriendlyName.ToLower().Replace(".exe", ""), @"\");
        }

        internal static string GetGameModeTitle()
        {
            return OrchestratorManager.GameMode switch
            {
                GameMode.Blink => LanguageManager.Get("BLINK_MODE"),
                GameMode.BlinkPlus => LanguageManager.Get("BLINK_MODE_PLUS"),
                GameMode.Rotate => LanguageManager.Get("ROTATE_MODE"),
                GameMode.RotatePlus => LanguageManager.Get("ROTATE_MODE_PLUS"),
                GameMode.Classic => LanguageManager.Get("CLASSIC_MODE"),
                GameMode.ClassicPlus => LanguageManager.Get("CLASSIC_MODE_PLUS"),
                GameMode.Endless => LanguageManager.Get("ENDLESS_MODE"),
                GameMode.EndlessPlus => LanguageManager.Get("ENDLESS_MODE_PLUS"),
                GameMode.Incremental => LanguageManager.Get("INCREMENTAL_MODE"),
                GameMode.IncrementalPlus => LanguageManager.Get("INCREMENTAL_MODE_PLUS"),
                GameMode.Memory => LanguageManager.Get("MEMORY_MODE"),
                GameMode.MemoryPlus => LanguageManager.Get("MEMORY_MODE_PLUS"),
                GameMode.Move => LanguageManager.Get("MOVE_MODE"),
                GameMode.MovePlus => LanguageManager.Get("MOVE_MODE_PLUS"),
                GameMode.TimeTrial => LanguageManager.Get("TIMETRIAL_MODE"),
                GameMode.TimeTrialPlus => LanguageManager.Get("TIMETRIAL_MODE_PLUS"),
                _ => string.Empty,
            };
        }

        internal static string GetHowToPlayDescription(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.Classic => LanguageManager.Get("HTP_CLASSIC_MODE"),
                GameMode.Incremental => LanguageManager.Get("HTP_INCREMENTAL_MODE"),
                GameMode.Endless => LanguageManager.Get("HTP_ENDLESS_MODE"),
                GameMode.Move => LanguageManager.Get("HTP_MOVE_MODE"),
                GameMode.Memory => LanguageManager.Get("HTP_MEMORY_MODE"),
                GameMode.Blink => LanguageManager.Get("HTP_BLINK_MODE"),
                GameMode.TimeTrial => LanguageManager.Get("HTP_TIMETRIAL_MODE"),
                GameMode.Rotate => LanguageManager.Get("HTP_ROTATE_MODE"),
                GameMode.ClassicPlus => LanguageManager.Get("HTP_CLASSIC_MODE_PLUS"),
                GameMode.IncrementalPlus => LanguageManager.Get("HTP_INCREMENTAL_MODE_PLUS"),
                GameMode.EndlessPlus => LanguageManager.Get("HTP_ENDLESS_MODE_PLUS"),
                GameMode.MovePlus => LanguageManager.Get("HTP_MOVE_MODE_PLUS"),
                GameMode.MemoryPlus => LanguageManager.Get("HTP_MEMORY_MODE_PLUS"),
                GameMode.BlinkPlus => LanguageManager.Get("HTP_BLINK_MODE_PLUS"),
                GameMode.TimeTrialPlus => LanguageManager.Get("TP_TIMETRIAL_MODE_PLUS"),
                GameMode.RotatePlus => LanguageManager.Get("HTP_ROTATE_MODE_PLUS"),
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

        internal static string GetAppVersion()
        {
#if ANDROID
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
            
#endif
        }

        /// <summary>
        /// Comprueba si hay coneción a internet.
        /// Solucionado el obsoleto.
        /// </summary>
        /// <returns></returns>
        internal static bool CheckConectivity()
        {
#if ANDROID
            ConnectivityManager connectivityManager = (ConnectivityManager)AndroidApp.Application.Context.GetSystemService(Context.ConnectivityService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Network network = connectivityManager.ActiveNetwork;

                if (network == null)
                    return false;

                NetworkCapabilities networkCapabilities = connectivityManager.GetNetworkCapabilities(network);

                return networkCapabilities != null && (networkCapabilities.HasTransport(TransportType.Wifi) || networkCapabilities.HasTransport(TransportType.Cellular));
            }
            else
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
                bool isConnected = (activeNetworkInfo != null) && activeNetworkInfo.IsConnected;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                bool hasInternetConnection = isConnected/* && CheckInternetConnectivity().GetAwaiter().GetResult()*/;
                return hasInternetConnection;
            }
#else
            
#endif
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
        /// el obsoleto en este método está solucionado.
        /// </summary>
        /// <param name="milliseconds"></param>
        internal static void Vibrate(int milliseconds)
        {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            var vibrator = (Vibrator)AndroidApp.Application.Context.GetSystemService(Context.VibratorService);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            if (vibrator.HasVibrator && UserSettingsManager.Vibration)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)/*Android 8.0 (API nivel 26) en adelante*/
                {
                    var vibrationEffect = VibrationEffect.CreateOneShot(milliseconds, VibrationEffect.DefaultAmplitude);
                    vibrator.Vibrate(vibrationEffect);
                }
                else
                {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                    vibrator.Vibrate(milliseconds);/*Para versiones anteriores a Android 8.0*/
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                }
            }
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
        /// Permanecerá la ejecucíon retenida por el tiempo indicado en milisegundos.
        /// </summary>
        /// <returns></returns>
        internal static void TimeStop(int miliseconds)
        {
            if (miliseconds == 0)
                return;

            Stopwatch stopwatch = new();
            stopwatch.Start();
            DateTime startDate = DateTime.Now;

            while (true)
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;

                if (elapsedTime.TotalMilliseconds >= miliseconds)
                {
                    stopwatch.Stop();
                    return;
                }

                SpinWait.SpinUntil(() => false, 100);
            }
        }

        internal static Texture2D GetPerkImage(PerkType perkType)
        {
            return perkType switch
            {
                PerkType.Reveal => TextureManager.TexturePerkReveal,
                PerkType.TimeStop => TextureManager.TexturePerkTimeStop,
                PerkType.Change => TextureManager.TexturePerkChange,
                _ => null,
            };
        }

        #endregion
    }
}
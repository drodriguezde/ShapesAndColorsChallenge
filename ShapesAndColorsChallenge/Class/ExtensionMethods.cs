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
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Android.App;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ShapesAndColorsChallenge.Class
{
    internal static class ExtensionMethods
    {
        #region METHODS

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas.
        /// </summary>
        /// <param name="id">Identificador entero único de la cadena</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string GetString(this int id)
        {
            return Application.Context.Resources.GetString(id);
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="id">Identificador entero único de la cadena</param>
        /// <param name="param1">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string GetString(this int id, int param1)
        {
            return Application.Context.Resources.GetString(id, param1);
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="id">Identificador entero único de la cadena</param>
        /// <param name="param1">Parámetro que se sustituirá</param>
        /// <param name="param2">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string GetString(this int id, int param1, int param2)
        {
            return Application.Context.Resources.GetString(id, param1, param2);
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="id">Identificador entero único de la cadena</param>
        /// <param name="param1">Parámetro que se sustituirá</param>
        /// <param name="param2">Parámetro que se sustituirá</param>
        /// <param name="param3">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string GetString(this int id, int param1, int param2, int param3)
        {
            return Application.Context.Resources.GetString(id, param1, param2, param3);
        }

        /// <summary>
        /// Obtiene una cadena de texto de un recurso de cadenas con un parámetro a sustituir.
        /// </summary>
        /// <param name="id">Identificador entero único de la cadena</param>
        /// <param name="param">Parámetro que se sustituirá</param>
        /// <returns>texto asociado al identificador</returns>
        internal static string GetString(this int id, string param)
        {
            return Application.Context.Resources.GetString(id, param);
        }

        /// <summary>
        /// Divide un valor entre dos.
        /// </summary>
        /// <param name="value">Valor a dividir.</param>
        /// <returns>Resultado.</returns>
        internal static float Half(this float value)
        {
            return value / 2;
        }

        /// <summary>
        /// Divide un valor entre dos.
        /// </summary>
        /// <param name="value">Valor a dividir.</param>
        /// <returns>Resultado.</returns>
        internal static int Half(this int value)
        {
            return value / 2;
        }

        /// <summary>
        /// Divide un valor entre dos.
        /// </summary>
        /// <param name="value">Valor a dividir.</param>
        /// <returns>Resultado.</returns>
        internal static Vector2 Half(this Vector2 value)
        {
            return value / 2;
        }

        /// <summary>
        /// Divide un valor entre tres.
        /// </summary>
        /// <param name="value">Valor a dividir.</param>
        /// <returns>Resultado.</returns>
        internal static int Third(this int value)
        {
            return value / 3;
        }

        /// <summary>
        /// Multiplica un valor por dos.
        /// </summary>
        /// <param name="value">Valor a multiplicar.</param>
        /// <returns>Resultado.</returns>
        internal static int Double(this int value)
        {
            return value * 2;
        }

        internal static float Double(this float value)
        {
            return value * 2;
        }

        /// <summary>
        /// Multiplica un valor por tres.
        /// </summary>
        /// <param name="value">Valor a multiplicar.</param>
        /// <returns>Resultado.</returns>
        internal static int Triple(this int value)
        {
            return value * 3;
        }

        /// <summary>
        /// Multiplica un valor por el multiplicador indicado.
        /// </summary>
        /// <param name="value">Valor a multiplicar.</param>
        /// <returns>Resultado.</returns>
        internal static int Multi(this int value, int multiplicator)
        {
            return value * multiplicator;
        }

        /// <summary>
        /// Divide un valor por el divisor indicado.
        /// </summary>
        /// <param name="value">Valor a dividir.</param>
        /// <returns>Resultado.</returns>
        internal static int Divide(this int value, int divider)
        {
            return value / divider;
        }

        internal static Vector2 ToVector2(this Point value)
        {
            return new Vector2(value.X, value.Y);
        }

        internal static Vector2 ToVector2(this Vector3 value)
        {
            return new Vector2(value.X, value.Y);
        }

        internal static Vector2 ToVector2(this Size value)
        {
            return new Vector2(value.Width, value.Height);
        }

        internal static Vector2 ToVector2(this int value)
        {
            return new Vector2(value, value);
        }

        internal static Point ToPoint(this Vector2 value)
        {
            return new Point(value.X.ToInt(), value.Y.ToInt());
        }

        internal static float ToSingle(this int value)
        {
            return Convert.ToSingle(value);
        }

        internal static float ToSingle(this double value)
        {
            return Convert.ToSingle(value);
        }

        internal static float ToSingle(this string value)
        {
            return Convert.ToSingle(value);
        }

        internal static double ToDouble(this float value)
        {
            return Convert.ToDouble(value);
        }

        internal static int Floor(this float value)
        {
            return ((float)Math.Floor(value)).ToInt();
        }

        internal static int Ceiling(this float value)
        {
            return ((float)Math.Ceiling(value)).ToInt();
        }

        internal static int Ceiling(this double value)
        {
            return Math.Ceiling(value).ToInt();
        }

        internal static int ToInt(this float value)
        {
            return Convert.ToInt32(value);
        }

        internal static int ToInt(this double value)
        {
            return Convert.ToInt32(value);
        }

        internal static int ToInt(this object value)
        {
            return Convert.ToInt32(value);
        }

        internal static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        internal static int ToInt(this bool value)
        {
            return Convert.ToInt32(value);
        }

        internal static Vector2 ToInt(this Vector2 value)
        {
            return new(value.X.ToInt(), value.Y.ToInt());
        }

        internal static bool ToBool(this string value)
        {
            return Convert.ToBoolean(value.ToInt());
        }

        internal static float Round1(this float value)
        {
            return Math.Round(value.ToDouble(), 1).ToSingle();
        }

        internal static float Round2(this float value)
        {
            return Math.Round(value.ToDouble(), 2).ToSingle();
        }

        internal static Point ToPoint(this Vector3 value)
        {
            return new Point(value.X.ToInt(), value.Y.ToInt());
        }

        internal static VertexPositionColor Clone(this VertexPositionColor value)
        {
            return new VertexPositionColor(new Vector3(value.Position.X, value.Position.Y, value.Position.Z), new Color(value.Color.R, value.Color.G, value.Color.B, value.Color.A));
        }

        internal static Vector2 OxaniumCorrection(this Vector2 value, int heightCorrection)
        {
            return new Vector2(value.X, value.Y - heightCorrection);
        }

        internal static Rectangle Clone(this Rectangle value)
        {
            return new Rectangle(value.X, value.Y, value.Width, value.Height);
        }

        internal static double Round1(this double value)
        {
            return Math.Round(value, 1);
        }

        internal static double Round2(this double value)
        {
            return Math.Round(value, 2);
        }

        internal static double Round3(this double value)
        {
            return Math.Round(value, 3);
        }

        internal static Point ToPointXNA(this System.Drawing.Point value)
        {
            return new Point(value.X, value.Y);
        }

        /// <summary>
        /// Comprueba su un número es impar.
        /// </summary>
        /// <param name="value">True, es impar.</param>
        /// <returns></returns>
        internal static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        internal static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        internal static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Convierte de grados a radianes.
        /// </summary>
        /// <param name="degrees">Grados a convertir.</param>
        /// <returns>Radianes.</returns>
        internal static float ToRadians(this float degrees)
        {
            return (degrees * Math.PI / 180).ToSingle();
        }

        internal static bool IsIn<T>(this T @this, params T[] possibles)
        {
            return possibles.Contains(@this);
        }

        public static string ToStringCulture(this float value)
        {
            return value.ToString(Statics.GetCultureInfo());
        }

        public static Size ToSize(this Rectangle value)
        {
            return new(value.Width, value.Height);
        }

        /// <summary>
        /// Devuelve el menor de una serie de valores o el valor de base.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int Min(this int value, params int[] values)
        {
            int min = value;

            for (int i = 0; i < values.Length; i++)
                if (values[i] < min)
                    min = values[i];

            return min;
        }

        /// <summary>
        /// Revuelve el rectangulo que está centrado dentro de otro con unas dimensiones determinadas.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rectangle CenterMe(this Rectangle value, float width, float height)
        {
            return new(
                (value.X + value.Width.Half() - width.Half()).ToInt(),
                (value.Y + value.Height.Half() - height.Half()).ToInt(),
                width.ToInt(),
                height.ToInt());
        }

        public static bool IsZero(this int value)
        {
            return value == 0;
        }

        public static bool NotIsZero(this int value)
        {
            return value != 0;
        }

        /// <summary>
        /// Indica si el modo es contrareloj.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTimeTrial(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.TimeTrial, Enum.GameMode.TimeTrialPlus);
        }

        /// <summary>
        /// Indica si el modo es memoria.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMemory(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Memory, Enum.GameMode.MemoryPlus);
        }

        /// <summary>
        /// Indica si el modo es incremental.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIncremental(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Incremental, Enum.GameMode.IncrementalPlus);
        }

        /// <summary>
        /// Indica si el modo es movimiento.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMovement(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Move, Enum.GameMode.MovePlus);
        }

        /// <summary>
        /// Indica si el modo es parpadeo.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBlink(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Blink, Enum.GameMode.BlinkPlus);
        }

        /// <summary>
        /// Indica si el modo es rotación.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRotate(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Rotate, Enum.GameMode.RotatePlus);
        }

        public static bool IsPlus(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.ClassicPlus,
                                Enum.GameMode.EndlessPlus,
                                Enum.GameMode.MemoryPlus,
                                Enum.GameMode.TimeTrialPlus,
                                Enum.GameMode.IncrementalPlus,
                                Enum.GameMode.MovePlus,
                                Enum.GameMode.BlinkPlus,
                                Enum.GameMode.RotatePlus);
        }

        /// <summary>
        /// Indica si el modo tiene fichas a encontrar ilimitadas.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasUnlimitedTiles(this Enum.GameMode value)
        {
            return value.IsIn(Enum.GameMode.Endless, Enum.GameMode.EndlessPlus, Enum.GameMode.TimeTrial, Enum.GameMode.TimeTrialPlus, Enum.GameMode.Incremental, Enum.GameMode.IncrementalPlus);
        }

        /// <summary>
        /// Indica si la forma se puede rotar sin que se salga de contenedor.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CanRotate(this Enum.ShapeType value)
        {
            return value.IsIn(Enum.ShapeType.Oval,
                                Enum.ShapeType.Pentagon,
                                Enum.ShapeType.Rombus,
                                Enum.ShapeType.Heart,
                                Enum.ShapeType.Star,
                                Enum.ShapeType.Diamond,
                                Enum.ShapeType.Moon,
                                Enum.ShapeType.Asterisk,
                                Enum.ShapeType.SixStar,
                                Enum.ShapeType.None);
        }

        /// <summary>
        /// Devuelve una ubicación aleatoria dentro de un rectángulo.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2 GetRandomLocationInside(this Rectangle rectangle)
        {
            return new(Statics.GetRandom(rectangle.X, rectangle.Right), Statics.GetRandom(rectangle.Y, rectangle.Bottom));
        }

        /// <summary>
        /// Obtiene una coordenada fuera del rectángulo.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 GetRandomLocationOutside(this Rectangle rectangle, int offset = 50)
        {
            int side = Statics.GetRandom(0, 3);

            return side switch
            {
                /*Izquierda*/
                0 => new(rectangle.X - offset, Statics.GetRandom(rectangle.Y, rectangle.Bottom)),
                /*Derecha*/
                1 => new(rectangle.Right + offset, Statics.GetRandom(rectangle.Y, rectangle.Bottom)),
                /*Arriba*/
                2 => new(Statics.GetRandom(rectangle.X, rectangle.Right), rectangle.Y - offset),
                /*Abajo*/
                3 => new(Statics.GetRandom(rectangle.X, rectangle.Right), rectangle.Bottom + offset),
                _ => Vector2.Zero,
            };
        }

        /// <summary>
        /// Comprueba si un vector es aproximadamente otro.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsAproximate(this Vector2 v1, Vector2 v2, float tolerance)
        {
            float sqrMagnitude = (v1 - v2).LengthSquared();
            float sqrTolerance = tolerance * tolerance;
            return sqrMagnitude <= sqrTolerance;
        }

        /// <summary>
        /// Reescala un rectángulo.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        internal static Rectangle RescaleRectangle(this Rectangle rectangle, float scale)
        {
            float newWidth = rectangle.Width.ToSingle() * scale;
            float newHeight = rectangle.Height.ToSingle() * scale;
            float newX = rectangle.X - (newWidth - rectangle.Width.ToSingle()) / 2;
            float newY = rectangle.Y - (newHeight - rectangle.Height.ToSingle()) / 2;

            return new Rectangle(newX.ToInt(), newY.ToInt(), newWidth.ToInt(), newHeight.ToInt());
        }

        #endregion
    }
}
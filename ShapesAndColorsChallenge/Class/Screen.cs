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
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class
{
    internal static class Screen
    {
        #region VARS



        #endregion

        #region DELEGATES


        #endregion

        #region PROPERTIES

        static Main Main { get; set; }

        /// <summary>
        /// Esta es la resolución real final, no siempre pone la deseada, por la barra de herramientas.
        /// </summary>
        internal static Size Resolution { get; set; }

        /// <summary>
        /// Matriz de redimensionado, sirve para escalar los sprites y otros objetos Con respecto a la densidad base xxxhdpi.
        /// Es decir, los objetos que tienen valores establecidos para xxxhdpi, como las ventanas  o los sprites.
        /// </summary>
        internal static Vector2 RedimMatrix { get; private set; }

        internal static Rectangle Bounds { get { return GraphicsDevice.Viewport.Bounds; } }

        internal static SpriteBatch SpriteBatch { get; set; }

        internal static GraphicsDevice GraphicsDevice { get; set; }

        internal static GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// Indica si el juego está áctivo.
        /// </summary>
        internal static bool IsActive { get; set; } = true;

        /// <summary>
        /// Obtiene la densidad de puntos por pulgada de la pantalla del dispositivo.
        /// </summary>
        internal static DPI GetDPI
        {
            get
            {
                int dpi = (int)Application.Context.Resources.DisplayMetrics.DensityDpi;

                if (dpi <= 120)
                    return DPI.ldpi;
                else if (dpi <= 160)
                    return DPI.mdpi;
                else if (dpi <= 240)
                    return DPI.hdpi;
                else if (dpi <= 320)
                    return DPI.xhdpi;
                else if (dpi <= 480)
                    return DPI.xxhdpi;
                else
                    return DPI.xxxhdpi;
            }
        }

        #endregion

        #region METHODS

        internal static void Initialize(Main main)
        {
            Main = main;
        }

        internal static void SpriteBatchBeginUI()
        {
            SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.AnisotropicClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/);
        }

        static void SpriteBatchBeginSprite()
        {
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.PointClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/);
        }

        static void SpriteBatchBeginShader()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        }

        static void SpriteBatchBegin(Effect effect)
        {
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.PointClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/,
                null,
                null,
                effect,
                null);
        }

        static void SpriteBatchBegin(Matrix matrix)
        {
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.AnisotropicClamp/*Escalado con antialising, wrap para tiles*/,
                null,
                null,
                null,
                matrix);
        }

        /// <summary>
        /// Finaliza el SpriteBatch.
        /// </summary>
        internal static void SpriteBatchEnd()
        {
            SpriteBatch.End();
        }

        /// <summary>
        /// Finaliza el SpriteBatch actual e inicia uno de interfaz.
        /// </summary>
        internal static void SpriteBatchRestartUI()
        {
            SpriteBatchEnd();
            SpriteBatchBeginUI();
        }

        /// <summary>
        /// Finaliza el SpriteBatch actual e inicia uno de sprite.
        /// </summary>
        internal static void SpriteBatchRestartSprite()
        {
            SpriteBatchEnd();
            SpriteBatchBeginSprite();
        }

        /// <summary>
        /// Finaliza el SpriteBatch actual e inicia uno de sprite.
        /// </summary>
        internal static void SpriteBatchRestartShader()
        {
            SpriteBatchEnd();
            SpriteBatchBeginShader();
        }

        /// <summary>
        /// Finaliza el SpriteBatch actual e inicia uno de matríz.
        /// </summary>
        /// <param name="matrix"></param>
        internal static void SpriteBatchRestart(Matrix matrix)
        {
            SpriteBatchEnd();
            SpriteBatchBegin(matrix);
        }

        /// <summary>
        /// Finaliza el SpriteBatch actual e inicia uno de efectos.
        /// </summary>
        /// <param name="effect"></param>
        internal static void SpriteBatchRestart(Effect effect)
        {
            SpriteBatchEnd();
            SpriteBatchBegin(effect);
        }

        /// <summary>
        /// Obtiene la escala de redimensionado dependiendo de la densidad de puntos por pulgada del dispositivo.
        /// Todos los Sprite son para el tamaño máximo xxxhdpi, por lo que el dpi actual es menor hay que reescalar a un tamaño menor el spite.
        /// </summary>
        /// <returns></returns>
        internal static Vector2 GetDPIScale()
        {
            DPI dPI = GetDPI;

            return dPI switch
            {
                DPI.ldpi => new Vector2(0.1875f, 0.1875f),
                DPI.mdpi => new Vector2(0.25f, 0.25f),
                DPI.hdpi => new Vector2(0.375f, 0.375f),
                DPI.xhdpi => new Vector2(0.5f, 0.5f),
                DPI.xxhdpi => new Vector2(0.75f, 0.75f),
                DPI.xxxhdpi => Vector2.One,
                _ => Vector2.One,
            };
        }

        internal static void SetRedimMatrix()
        {
            RedimMatrix = new Vector2(
                Bounds.Width.ToSingle() / BaseBounds.Bounds.Width.ToSingle(),
                Bounds.Height.ToSingle() / BaseBounds.Bounds.Height.ToSingle());
        }

        /// <summary>
        /// Pone la aplicación en pantalla completa, y la ajusta a las medidas y coordenas correctas.
        /// Inicialmente pone unas corrdenadas de inicio diferentes a x = 0, y = 0 y una dimensión de viewport que no es la full.
        /// </summary>
        internal static void ToggleFullScreen()
        {
            Size maxScreenUserSize = new(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            ChangeResolution(maxScreenUserSize);

            if (!Graphics.IsFullScreen)
                Graphics.ToggleFullScreen();/*Cambia el modo a pantalla completa si no lo está y viceversa*/

            Graphics.ApplyChanges();
            SetRedimMatrix();
        }

        internal static void ChangeResolution(Size changeToResolution)
        {
            Graphics.PreferredBackBufferWidth = changeToResolution.Width;
            Graphics.PreferredBackBufferHeight = changeToResolution.Height;
            Graphics.ApplyChanges();
        }

        internal static void DeploySpriteBatch()
        {
            try
            {
                SpriteBatch = new SpriteBatch(GraphicsDevice);
            }
            catch (Exception exception)
            {
                Statics.TraceException(exception.Message);
            }
        }

        internal static Vector2 Redim(this Vector2 value)
        {
            return new Vector2(value.X * RedimMatrix.X, value.Y * RedimMatrix.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Valor original en 1920x1060</param>
        /// <returns>Valor convertido a la nueva resolución</returns>
        internal static Rectangle Redim(this Rectangle value)
        {
            return new Rectangle(
                (value.X * RedimMatrix.X).ToInt(),
                (value.Y * RedimMatrix.Y).ToInt(),
                (value.Width * RedimMatrix.X).ToInt(),
                (value.Height * RedimMatrix.Y).ToInt());
        }

        internal static int RedimX(this int value)
        {
            return (value * RedimMatrix.X).ToInt();
        }

        internal static int RedimY(this int value)
        {
            return (value * RedimMatrix.Y).ToInt();
        }

        internal static int RedimY(this long value)
        {
            return (value * RedimMatrix.Y).ToInt();
        }

        internal static float RedimX(this float value)
        {
            return value * RedimMatrix.X;
        }

        internal static float RedimY(this float value)
        {
            return value * RedimMatrix.Y;
        }

        internal static Point RedimToPoint(this Vector2 value)
        {
            return new Point((value.X * RedimMatrix.X).ToInt(), (value.Y * RedimMatrix.Y).ToInt());
        }

        internal static Vector2 RedimToVector2(this Point value)
        {
            return new Vector2(value.X * RedimMatrix.X, value.Y * RedimMatrix.Y);
        }

        internal static Point Redim(this Point value)
        {
            return new Point((value.X * RedimMatrix.X).ToInt(), (value.Y * RedimMatrix.Y).ToInt());
        }

        internal static VertexPositionColor Redim(this VertexPositionColor value)
        {
            return new VertexPositionColor(new Vector3(RedimX(value.Position.X), RedimY(value.Position.Y), value.Position.Z), value.Color);
        }

        internal static Texture2D ToBorderedRectangle(Rectangle bounds, Color innerColor, Color borderColor)
        {
            Texture2D rectangle = new(GraphicsDevice, bounds.Width, bounds.Height);
            Color[] dataColor = new Color[bounds.Width * bounds.Height];

            for (int i = 0; i < dataColor.Length; i++)/*Todo del color interior*/
                dataColor[i] = innerColor;

            for (int i = 0; i < bounds.Width; i++)/*Borde superior*/
                dataColor[i] = borderColor;

            for (int i = dataColor.Length - 1; i > dataColor.Length - 1 - bounds.Width; i--)/*Borde inferior*/
                dataColor[i] = borderColor;

            for (int i = 0; i < dataColor.Length; i += bounds.Width)/*Borde derecho*/
                dataColor[i] = borderColor;

            for (int i = bounds.Width - 1; i < dataColor.Length; i += bounds.Width)/*Borde izquierdo*/
                dataColor[i] = borderColor;

            rectangle.SetData(dataColor);
            return rectangle;
        }

        internal static Texture2D ToCircle(int radius, Color color)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new(GraphicsDevice, outerRadius, outerRadius);
            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));
                data[y * outerRadius + x + 1] = color;
            }

            texture.SetData(data);
            return texture;
        }

        internal static Texture2D ToCircle(int radius, Color innerColor, Color borderColor)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new(GraphicsDevice, outerRadius, outerRadius);
            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = borderColor;
            }

            //width
            for (int i = 0; i < outerRadius; i++)
            {
                int yStart = -1;
                int yEnd = -1;

                //loop through height to find start and end to fill
                for (int j = 0; j < outerRadius; j++)
                {
                    if (yStart == -1)
                    {
                        if (j == outerRadius - 1)
                        {
                            //last row so there is no row below to compare to
                            break;
                        }

                        //start is indicated by Color followed by Transparent
                        if (data[i + (j * outerRadius)] == borderColor && data[i + ((j + 1) * outerRadius)] == Color.Transparent)
                        {
                            yStart = j + 1;
                            continue;
                        }
                    }
                    else if (data[i + (j * outerRadius)] == borderColor)
                    {
                        yEnd = j;
                        break;
                    }
                }

                //if we found a valid start and end position
                if (yStart != -1 && yEnd != -1)
                    //height
                    for (int j = yStart; j < yEnd; j++)
                        data[i + (j * outerRadius)] = innerColor;
            }

            texture.SetData(data);
            return texture;
        }

        internal static Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new(GraphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int pixel = 0; pixel < data.Length; pixel++)
                data[pixel] = paint(pixel);

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// Obtiene una textura con la imagen actual mostrada por el juego.
        /// </summary>
        /// <returns></returns>
        internal static Texture2D TakeScreenshot()
        {
            RenderTarget2D screenshot;
            screenshot = new RenderTarget2D(GraphicsDevice, Bounds.Width, Bounds.Height, false, SurfaceFormat.Color, DepthFormat.None);
            GraphicsDevice.SetRenderTarget(screenshot);
            Main.TakeScreenshot(new GameTime());
            GraphicsDevice.SetRenderTarget(null);
            return screenshot;
        }

        /// <summary>
        /// Obtiene la escala que hay entre dos vectores, para que aplicada al segundo, sea este igual que el primero.
        /// </summary>
        /// <returns></returns>
        internal static Vector2 GetScaleToFit(Vector2 origin, Vector2 destination)
        {
            return new Vector2(destination.X / origin.X, destination.Y / origin.Y);
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
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
        /// Matriz de redimensionado, sirve para escalar los sprites y otros objetos con respecto a la densidad base 440.
        /// Es decir, los objetos que tienen valores establecidos para 440, como las ventanas  o los sprites.
        /// </summary>
        static Matrix RedimMatrix { get; set; }

        static Rectangle Bounds { get { return GraphicsDevice.Viewport.Bounds; } }

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
        internal static int GetDPI
        {
            get
            {
                return (int)Application.Context.Resources.DisplayMetrics.DensityDpi;
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
            SetRedimMatrix();
            SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.AnisotropicClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/,
                transformMatrix: RedimMatrix);
        }

        static void SpriteBatchBeginSprite()
        {
            SetRedimMatrix();
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.PointClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/,
                transformMatrix: RedimMatrix);
        }

        static void SpriteBatchBeginShader()
        {
            SetRedimMatrix();
            SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                transformMatrix: RedimMatrix);
        }

        static void SpriteBatchBegin(Effect effect)
        {
            SetRedimMatrix();
            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend/*Si se cambia no funcionan los onhover*/,
                SamplerState.PointClamp/*Escalado sin flurry, pixel perfecto, wrap para tiles*/,
                null,
                null,
                effect,
                transformMatrix: RedimMatrix);
        }

        static void SpriteBatchBegin(Matrix matrix)
        {
            SetRedimMatrix();
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

        internal static void SetResolution()
        {
            Resolution = new Size(Graphics.PreferredBackBufferHeight, Graphics.PreferredBackBufferWidth);
            SetRedimMatrix();
        }

        internal static void SetRedimMatrix()
        {
            float scaleX = (Bounds.Width / GetDPI) / (BaseBounds.Bounds.Width / BaseBounds.DPI);
            float scaleY = (Bounds.Height / GetDPI) / (BaseBounds.Bounds.Height / BaseBounds.DPI);
            RedimMatrix = Matrix.CreateScale(scaleX, scaleY, 1f);
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

        #endregion
    }
}
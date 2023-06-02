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

        static float offsetX = 0f;
        static float offsetY = 0f;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Esta es la resolución real final, no siempre pone la deseada, por la barra de herramientas.
        /// </summary>
        internal static Size Resolution { get; set; }

        internal static float OffsetX { get { return offsetX; } }

        /// <summary>
        /// Matriz de redimensionado, sirve para escalar los sprites y otros objetos con respecto a la densidad base 440.
        /// Es decir, los objetos que tienen valores establecidos para 440, como las ventanas  o los sprites.
        /// </summary>
        static Matrix RedimMatrix { get; set; }

        internal static Rectangle Bounds { get { return GraphicsDevice.Viewport.Bounds; } }

        internal static Rectangle BacklayerBounds { get { return new(-1000, -1000, Bounds.Width + 2000, Bounds.Height + 2000); } }

        /// <summary>
        /// Cuando la pantalla es de diferente tamaño a la BaseBounds para que no se deformen los objetos se aplica un offset arriba y a la derecha para centrar todo.
        /// BoundsOffset añade el offset para no perderlo en cada dibujado.  
        /// </summary>
        internal static Rectangle BoundsOffset { get { return new(Bounds.X - offsetX.ToInt(), Bounds.Y - offsetY.ToInt(), Bounds.Width, Bounds.Height); } }

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
        internal static float GetDPI
        {
            get
            {
                return (float)Application.Context.Resources.DisplayMetrics.DensityDpi;
            }
        }

        #endregion

        #region METHODS

        internal static void Initialize()
        {
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
            float scaleX = Bounds.Width.ToSingle() / BaseBounds.Bounds.Width.ToSingle();
            float scaleY = Bounds.Height.ToSingle() / BaseBounds.Bounds.Height.ToSingle();
            float minScale = Math.Min(scaleX, scaleY);
            offsetX = (Bounds.Width.ToSingle() - (BaseBounds.Bounds.Width.ToSingle() * minScale)) / 2f;
            offsetY = (Bounds.Height.ToSingle() - (BaseBounds.Bounds.Height.ToSingle() * minScale)) / 2f;
            Matrix translationMatrix = Matrix.CreateTranslation(offsetX, offsetY, 0f);
            Matrix scaleMatrix = Matrix.CreateScale(minScale, minScale, 1f);
            RedimMatrix = scaleMatrix * translationMatrix;
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

        internal static Rectangle Rescale(this Rectangle rectangle)
        {
            Vector2 topLeft = new(rectangle.Left, rectangle.Top);
            Vector2 topRight = new(rectangle.Right, rectangle.Top);
            Vector2 bottomLeft = new(rectangle.Left, rectangle.Bottom);
            Vector2 bottomRight = new(rectangle.Right, rectangle.Bottom);
            topLeft = Vector2.Transform(topLeft, RedimMatrix);
            topRight = Vector2.Transform(topRight, RedimMatrix);
            bottomLeft = Vector2.Transform(bottomLeft, RedimMatrix);
            bottomRight = Vector2.Transform(bottomRight, RedimMatrix);

            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            int newWidth = (int)Math.Round(maxX - minX);
            int newHeight = (int)Math.Round(maxY - minY);
            int newX = (int)Math.Round(minX);
            int newY = (int)Math.Round(minY);

            return new(newX, newY, newWidth, newHeight);
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
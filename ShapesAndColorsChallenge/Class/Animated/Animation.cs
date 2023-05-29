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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.Effects.Bloom;
using ShapesAndColorsChallenge.Class.Management;
using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Animated
{
    internal class Animation : IDisposable
    {
        #region PROPERTIES

        /// <summary>
        /// Número de frames deseados para completar la animación.
        /// Ejemplo: si una animación tiene solo 4 frames y la tasa de frames en pantalla son 60 por segundo, la animación será inperceptible.
        /// La relación entre esta propiedad y FramesOfTheAnimation hacen que sea perceptible ya que se repetiran frames para darle fluidez y perceptibilidad.
        /// </summary>
        int FramesNeededToCompleteTheAnimation { get; set; } = Const.GAME_FRAME_RATE;

        /// <summary>
        /// Tiempo que durará cada bucle, en milisegundos.
        /// </summary>
        internal int TimeLoop { get; set; } = 0;

        /// <summary>
        /// Número total de frames que harán falta para completar un bucle de animación.
        /// </summary>
        internal int FramesOfTheAnimation { get; set; }

        /// <summary>
        /// Tiempo que hay entre frames.
        /// </summary>
        protected int TimePerFrame { get; set; }

        /// <summary>
        /// Indica si la animación está en marcha.
        /// </summary>
        internal bool Running { get; set; } = false;

        /// <summary>
        /// Indica el frame que está en marcha actualmente.
        /// </summary>
        internal int CurrentFrame { get; set; } = 0;

        /// <summary>
        /// Momento del último pase de animación.
        /// </summary>
        internal TimeSpan LastFrameTime { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Posición del punto central.
        /// </summary>
        protected Vector2 OriginalCenterLocation { get; set; } = Vector2.Zero;

        protected Rectangle OriginalBounds { get; set; } = Rectangle.Empty;

        protected Vector2 OriginalLocation { get; set; } = Vector2.Zero;

        protected Vector2 OriginalSize { get; set; }

        internal InteractiveObject Object { get; set; }

        List<Texture2D> Sprites { get; set; }

        /// <summary>
        /// Esta textura contendrá el efecto shader.
        /// </summary>
        Texture2D EffectTexture { get; set; }

        protected Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Rotación del objeto sobre su eje (RADIANES).
        /// </summary>
        internal float Rotation { get; set; } = 0f;

        /// <summary>
        /// Indica su hay que aplicar un efecto bloom.
        /// </summary>
        internal bool BloomEnabled { get; set; } = false;

        /// <summary>
        /// Indica que el efecto bloom solo se aplicará si BloomEnabled = true y la animación está en marcha.
        /// </summary>
        internal bool BloomEnabledOnlyInMotion { get; set; } = false;

        /// <summary>
        /// Indica que solo se debe mostrar el efecto bloom pero no la textura con que se genera.
        /// </summary>
        internal bool ShowOnlyBloom { get; set; } = false;

        /// <summary>
        /// Valores para el efecto bloom de esta animación;
        /// </summary>
        internal BloomValues BloomValues { get; set; }

        internal bool Visible { get; set; } = false;

        /// <summary>
        /// Indica que la animación se reproduce una sola vez.
        /// </summary>
        bool RunningOnce { get; set; } = false;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor para InteractiveObject
        /// </summary>
        /// <param name="interactiveObject"></param>
        internal Animation(InteractiveObject interactiveObject)
        {
            Object = interactiveObject;
            OriginalBounds = interactiveObject.Bounds;
            OriginalLocation = interactiveObject.Location;
            OriginalSize = new Vector2(OriginalBounds.Width, OriginalBounds.Height);
            OriginalCenterLocation = new Vector2(OriginalBounds.X + OriginalBounds.Width.Half(), OriginalBounds.Y + OriginalBounds.Height.Half());
        }

        /// <summary>
        /// Constructor para animar sprites.
        /// </summary>
        /// <param name="framesNeededToCompleteTheAnimation"></param>
        internal Animation(List<Texture2D> sprites, int framesNeededToCompleteTheAnimation, Vector2 location)
        {
            Sprites = sprites;
            FramesOfTheAnimation = Sprites.Count;
            FramesNeededToCompleteTheAnimation = framesNeededToCompleteTheAnimation;
            OriginalLocation = location;
            CalculateTimeBetweenFrames();
        }

        /// <summary>
        /// Constructor para animar sprites.
        /// </summary>
        /// <param name="framesNeededToCompleteTheAnimation"></param>
        internal Animation(List<Texture2D> sprites, int framesNeededToCompleteTheAnimation, Vector2 location, Vector2 scale)
            : this(sprites, framesNeededToCompleteTheAnimation, location)
        {
            Scale = scale;
        }

        /// <summary>
        /// Constructor para InteractiveObject
        /// </summary>
        /// <param name="interactiveObject"></param>
        /// <param name="timeLoop"></param>
        internal Animation(InteractiveObject interactiveObject, int timeLoop)
        {
            Object = interactiveObject;
            TimeLoop = timeLoop;
            OriginalBounds = interactiveObject.Bounds;
            OriginalLocation = interactiveObject.Location;
            OriginalSize = new Vector2(OriginalBounds.Width, OriginalBounds.Height);
            OriginalCenterLocation = new Vector2(OriginalBounds.X + OriginalBounds.Width.Half(), OriginalBounds.Y + OriginalBounds.Height.Half());
        }

        #endregion

        #region DESTRUCTOR

        /// <summary>
        /// Variable que indica si se ha destruido el objeto.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Libera todos los recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                EffectTexture?.Dispose();
            }

            /*Objetos no administrados aquí*/

            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Animation()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal void CalculateTimeBetweenFrames()
        {
            TimePerFrame = (FramesNeededToCompleteTheAnimation.ToSingle() / Const.GAME_FRAME_RATE.ToSingle() * 1000f / FramesOfTheAnimation.ToSingle()).ToInt();
        }

        internal virtual void Initialize()
        {

        }

        /// <summary>
        /// Reproduce la animación una sola vez.
        /// </summary>
        internal virtual void RunOnce()
        {
            Visible = true;
            Running = true;
            RunningOnce = true;
            CurrentFrame = 0;
            LastFrameTime = TimeSpan.Zero;
        }

        /// <summary>
        /// Comienza la animación desde el principio.
        /// </summary>
        internal virtual void Start()
        {
            Running = true;
        }

        /// <summary>
        /// Comienza la animación desde el frame indicado.
        /// Se usa sincronizar animaciones.
        /// </summary>
        internal virtual void Start(TimeSpan startTime)
        {
            Running = true;
            LastFrameTime = startTime;
        }

        /// <summary>
        /// Detiene la animación en el frame actual.
        /// </summary>
        internal virtual void Pause()
        {
            Running = false;
        }

        /// <summary>
        /// Reanuda la animación desde el frame en que se quedó.
        /// </summary>
        internal virtual void Resume()
        {
            CurrentFrame = 0;
        }

        /// <summary>
        /// Detiene la animación y la resetea al frame inicial.
        /// </summary>
        internal virtual void Stop()
        {
            Running = false;
            CurrentFrame = 0;
            LastFrameTime = TimeSpan.Zero;
        }

        bool IsRunningOnce()
        {
            if (!RunningOnce)
                return false;

            Visible = false;
            Running = false;
            RunningOnce = false;
            CurrentFrame = 0;
            LastFrameTime = TimeSpan.Zero;
            return true;
        }

        internal virtual void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            if (gameTime.TotalGameTime.Subtract(LastFrameTime).Milliseconds < TimePerFrame)
                return;

            LastFrameTime = gameTime.TotalGameTime;

            if (FramesOfTheAnimation == CurrentFrame + 1)
            {
                if (IsRunningOnce())
                    return;

                CurrentFrame = 0;
            }
            else
                CurrentFrame++;

            if (BloomEnabled)
                SetBloom();
        }

        internal virtual void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            AddBloom();

            if ((BloomEnabled && !ShowOnlyBloom) || !BloomEnabled)
                Screen.SpriteBatch.Draw(Sprites[CurrentFrame], OriginalLocation, null, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        void AddBloom()
        {
            if (BloomEnabled)
                if ((BloomEnabledOnlyInMotion && Running) || !BloomEnabledOnlyInMotion)
                    if (EffectTexture != null)
                    {
                        Screen.SpriteBatchRestartShader();
                        Screen.SpriteBatch.Draw(EffectTexture, OriginalLocation, Color.White);
                        Screen.SpriteBatchRestartUI();
                    }
        }

        void SetBloom()
        {
            ShaderManager.BloomFilter.SetValues(BloomValues);
            EffectTexture = ShaderManager.BloomFilter.Draw(Sprites[CurrentFrame], (Sprites[CurrentFrame].Width.ToSingle() * Scale.X).ToInt(), (Sprites[CurrentFrame].Height.ToSingle() * Scale.Y).ToInt());
            Screen.GraphicsDevice.SetRenderTarget(null);
        }

        #endregion
    }
}
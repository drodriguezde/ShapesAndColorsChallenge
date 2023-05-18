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
using ShapesAndColorsChallenge.Class.Controls;

namespace ShapesAndColorsChallenge.Class.Animated
{
    internal class AnimationHeartBeat : Animation
    {
        #region PROPERTIES

        /// <summary>
        /// Indica la escala del tipo de animación HeartBeat.
        /// Cuan grande se hará el texto en la animación.
        /// Valor por defecto 1.5f.
        /// </summary>
        internal float ScaleHeartBeat { get; set; } = 1.5f;

        /// <summary>
        /// Cuanto crece el texto por frame.
        /// </summary>
        float ScaleDiffPerFrame { get; set; } = 0f;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactiveObject">Objeto que se animará</param>
        /// <param name="timeLoop">Tiempo que durará cada bucle, en milisegundos</param>
        internal AnimationHeartBeat(InteractiveObject interactiveObject, int timeLoop)
            : base(interactiveObject, timeLoop)
        {
            FramesOfTheAnimation = 10;
            Initialize();
        }

        #endregion

        #region METHODS

        internal override void Initialize()
        {
            TimePerFrame = TimeLoop / FramesOfTheAnimation / 2;
            ScaleDiffPerFrame = ScaleHeartBeat.Half() / FramesOfTheAnimation / 4;
            CurrentFrame = 1;
        }

        /// <summary>
        /// Comienza la animación desde el principio.
        /// </summary>
        internal override void Start()
        {
            CurrentFrame = 0;
            Object.MasterScale = Vector2.One;
            Object.Bounds = OriginalBounds;
            Running = true;
        }

        /// <summary>
        /// Detiene la animación en el frame actual.
        /// </summary>
        internal override void Pause()
        {
            Running = false;
        }

        /// <summary>
        /// Reanuda la animación desde el frame en que se quedó.
        /// </summary>
        internal override void Resume()
        {
            Running = true;
        }

        /// <summary>
        /// Detiene la animación y la resetea al freme inicial.
        /// </summary>
        internal override void Stop()
        {
            CurrentFrame = 0;
            Object.MasterScale = Vector2.One;
            Object.Bounds = OriginalBounds;
            Running = false;
        }

        internal override void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            if (gameTime.TotalGameTime.Subtract(LastFrameTime).TotalMilliseconds < TimePerFrame)
                return;

            LastFrameTime = gameTime.TotalGameTime;
            CurrentFrame++;

            if (CurrentFrame == 21)
                CurrentFrame = 1;

            if (CurrentFrame < 11)/*Crece*/
                Object.MasterScale += new Vector2(ScaleDiffPerFrame, ScaleDiffPerFrame);
            else/*Mengua*/
                Object.MasterScale -= new Vector2(ScaleDiffPerFrame, ScaleDiffPerFrame);

            Vector2 newSize = OriginalSize * Object.MasterScale;
            Object.Bounds = new Rectangle(
                (OriginalCenterLocation.X - newSize.Half().X).ToInt(),
                (OriginalCenterLocation.Y - newSize.Y.Half()).ToInt(),
                newSize.X.ToInt(),
                newSize.Y.ToInt());
        }

        #endregion
    }
}
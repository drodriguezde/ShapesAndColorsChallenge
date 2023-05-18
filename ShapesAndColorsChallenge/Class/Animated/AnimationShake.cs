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
    internal class AnimationShake : Animation
    {
        #region CONST

        const int FRAMES = 16;

        #endregion

        #region PROPERTIES

        readonly float ShakeRange = BaseBounds.Bounds.Redim().Width * 0.005f < 1 ? 1 : BaseBounds.Bounds.Redim().Width * 0.005f;/*0.5% del ancho de la pantalla*/

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactiveObject">Objeto que se animará</param>
        /// <param name="timeLoop">Tiempo que durará cada bucle, en milisegundos</param>
        internal AnimationShake(InteractiveObject interactiveObject, int timeLoop)
            : base(interactiveObject, timeLoop)
        {
            FramesOfTheAnimation = FRAMES;
            Initialize();
        }

        #endregion

        #region METHODS

        internal override void Initialize()
        {
            TimePerFrame = TimeLoop / FramesOfTheAnimation;
            CurrentFrame = 1;
        }

        /// <summary>
        /// Comienza la animación desde el principio.
        /// </summary>
        internal override void Start()
        {
            CurrentFrame = 0;
            Object.Location = OriginalLocation;
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
            Object.Location = OriginalLocation;
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

            switch (CurrentFrame)
            {
                case 1:
                    Object.Location = new Vector2(OriginalLocation.X, OriginalLocation.Y - ShakeRange);
                    break;
                case 2:
                    Object.Location = new Vector2(OriginalLocation.X, OriginalLocation.Y + ShakeRange);
                    break;
                case 3:
                    Object.Location = new Vector2(OriginalLocation.X - ShakeRange, OriginalLocation.Y);
                    break;
                case 4:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y);
                    break;
                case 5:
                    Object.Location = new Vector2(OriginalLocation.X - ShakeRange, OriginalLocation.Y - ShakeRange);
                    break;
                case 6:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y + ShakeRange);
                    break;
                case 7:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y);
                    break;
                case 8:
                    Object.Location = new Vector2(OriginalLocation.X, OriginalLocation.Y + ShakeRange);
                    break;
                case 9:
                    Object.Location = new Vector2(OriginalLocation.X, OriginalLocation.Y - ShakeRange);
                    break;
                case 10:
                    Object.Location = new Vector2(OriginalLocation.X, OriginalLocation.Y + ShakeRange);
                    break;
                case 11:
                    Object.Location = new Vector2(OriginalLocation.X - ShakeRange, OriginalLocation.Y);
                    break;
                case 12:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y);
                    break;
                case 13:
                    Object.Location = new Vector2(OriginalLocation.X - ShakeRange, OriginalLocation.Y - ShakeRange);
                    break;
                case 14:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y + ShakeRange);
                    break;
                case 15:
                    Object.Location = new Vector2(OriginalLocation.X + ShakeRange, OriginalLocation.Y - ShakeRange);
                    break;
                case 16:
                    Object.Location = new Vector2(OriginalLocation.X - ShakeRange, OriginalLocation.Y + ShakeRange);
                    break;
                case 17:
                    Object.Location = OriginalLocation;
                    Running = false;
                    return;
            }
        }

        #endregion
    }
}
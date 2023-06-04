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

        readonly float ShakeRange = Screen.Bounds.Width * 0.005f < 1 ? 1 : Screen.Bounds.Width * 0.005f;/*0.5% del ancho de la pantalla*/

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
            Object.Origin = Vector2.Zero;
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
            Running = false;
            Object.Origin = Vector2.Zero;
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
                    Object.Origin = Vector2.Zero + new Vector2(0, -ShakeRange);
                    break;
                case 2:
                    Object.Origin = Vector2.Zero + new Vector2(0, ShakeRange);
                    break;
                case 3:
                    Object.Origin = Vector2.Zero + new Vector2(-ShakeRange, 0);
                    break;
                case 4:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, 0);
                    break;
                case 5:
                    Object.Origin = Vector2.Zero + new Vector2(-ShakeRange, -ShakeRange);
                    break;
                case 6:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, ShakeRange);
                    break;
                case 7:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, 0);
                    break;
                case 8:
                    Object.Origin = Vector2.Zero + new Vector2(0, ShakeRange);
                    break;
                case 9:
                    Object.Origin = Vector2.Zero + new Vector2(0, -ShakeRange);
                    break;
                case 10:
                    Object.Origin = Vector2.Zero + new Vector2(0, ShakeRange);
                    break;
                case 11:
                    Object.Origin = Vector2.Zero + new Vector2(-ShakeRange, 0);
                    break;
                case 12:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, 0);
                    break;
                case 13:
                    Object.Origin = Vector2.Zero + new Vector2(-ShakeRange, -ShakeRange);
                    break;
                case 14:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, ShakeRange);
                    break;
                case 15:
                    Object.Origin = Vector2.Zero + new Vector2(ShakeRange, -ShakeRange);
                    break;
                case 16:
                    Object.Origin = Vector2.Zero + new Vector2(-ShakeRange, ShakeRange);
                    break;
                case 17:
                    Object.Origin = Vector2.Zero;
                    Running = false;
                    return;
            }
        }

        #endregion
    }
}
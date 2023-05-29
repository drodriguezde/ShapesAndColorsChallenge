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
    internal class AnimationRotate : Animation
    {
        #region CONST



        #endregion

        #region PROPERTIES

        float RotationSpeed { get; set; } = 0f;

        /// <summary>
        /// Sentido de giro.
        /// </summary>
        bool RightToLeft { get; set; } = true;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactiveObject">Objeto que se animará</param>
        internal AnimationRotate(InteractiveObject interactiveObject, int framesOfTheAnimation, bool rightToLeft = false)
            : base(interactiveObject)
        {
            FramesOfTheAnimation = framesOfTheAnimation;
            RightToLeft = rightToLeft;
            Initialize();
        }

        #endregion

        #region METHODS

        internal override void Initialize()
        {
            RotationSpeed = MathHelper.TwoPi / FramesOfTheAnimation.ToSingle();
        }

        /// <summary>
        /// Comienza la animación desde el principio.
        /// </summary>
        internal override void Start()
        {
            Object.Rotation = 0;
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
            Running = false;
            Object.Rotation = 0;
        }

        internal override void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            if (RightToLeft)
            {
                Object.Rotation += RotationSpeed;

                if (Object.Rotation >= MathHelper.TwoPi)
                    Object.Rotation = 0f;
            }
            else
            {
                Object.Rotation -= RotationSpeed;

                if (Object.Rotation <= -MathHelper.TwoPi)
                    Object.Rotation = 0f;
            }

            Object.Location = new(Object.OriginalLocation.X + Object.OriginalBounds.Width.ToSingle().Half(), Object.OriginalLocation.Y + Object.OriginalBounds.Height.ToSingle().Half());
            Object.Origin = new(
                Object.Bounds.Width - Object.Bounds.Width.ToSingle().Half(),
                Object.Bounds.Height - Object.Bounds.Height.ToSingle().Half());
            Object.Origin /= Object.Scale;
        }

        #endregion
    }
}
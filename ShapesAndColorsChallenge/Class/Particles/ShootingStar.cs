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
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.Management;

namespace ShapesAndColorsChallenge.Class.Particles
{
    internal class ShootingStar : Entity
    {
        #region VARS

        /// <summary>
        /// Tiempo transcurrido desde el inicio.
        /// </summary>
        float elapsedTime = 0f;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Ubicación destino.
        /// </summary>
        Vector2 TargetLocation { get; set; }

        /// <summary>
        /// Tiempo que tendrá el la estrella de ir del inicio al final.
        /// </summary>
        float Speed
        {
            get
            {
                return BaseBounds.Bounds.Height / 60f;/*Debe recorrer el largo de la pantalla en 1 segundo (60 frames)*/
            }
        }

        /// <summary>
        /// Escala de la estrella.
        /// </summary>
        float Scale { get; set; }

        /// <summary>
        /// Velocidad de rotación de la estrella.
        /// </summary>
        float RotationSpeed { get; set; }

        /// <summary>
        /// Textura de la estrella.
        /// </summary>
        Texture2D Star = TextureManager.TextureStarMini;

        /// <summary>
        /// Posición actual de la estrella.
        /// </summary>
        Vector2 CurrentPosition { get; set; }

        /// <summary>
        /// Ángulo de rotación de la estrella.
        /// </summary>
        float RotationAngle { get; set; }

        /// <summary>
        /// Indica que se ha acabado.
        /// </summary>
        internal bool End { get; private set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal ShootingStar(Vector2 currentPosition, Vector2 targetLocation, float scale = 1f, float rotationSpeed = 4f)
        {
            CurrentPosition = currentPosition;
            TargetLocation = targetLocation;
            Scale = scale;
            RotationSpeed = rotationSpeed;
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        internal override void Update(GameTime gameTime)
        {
            if (End)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;
            elapsedTime = MathHelper.Clamp(elapsedTime, 0f, Speed);
            float t = elapsedTime / Speed;
            CurrentPosition = Vector2.Lerp(CurrentPosition, TargetLocation, t).ToInt();
            RotationAngle = elapsedTime * RotationSpeed;

            if (TargetLocation.IsAproximate(CurrentPosition, 20f))
            {
                SoundManager.PlayBallonPop();
                End = true;
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!End)
                Screen.SpriteBatch.Draw(Star, CurrentPosition, null, Color.White, RotationAngle, new(Star.Width.Half(), Star.Height.Half()), Scale, SpriteEffects.None, 0f);
        }

        #endregion
    }
}

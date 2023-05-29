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
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Particles.ConfettiParticle
{
    internal class ConfettiPaper : Entity
    {
        #region PROPERTIES

        /// <summary>
        /// Textura que tendrá el confeti.
        /// </summary>
        internal Texture2D Texture { get; set; }

        /// <summary>
        /// Escala de tamaño del confeti.
        /// </summary>
        internal Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Cantidad de transparencia del confeti.
        /// </summary>
        internal float Transparency { get; set; } = 1.0f;

        /// <summary>
        /// Color del confeti.
        /// </summary>
        internal Color Color { get; set; } = Color.White;

        /// <summary>
        /// Velocidad del confeti, de movimiento y de rotación, en x y en y.
        /// </summary>
        internal Vector2 Speed { get; set; }

        /// <summary>
        /// Aumento de velocidad por frame.
        /// </summary>
        internal Vector2 SpeedIncrement { get; set; } = Vector2.Zero;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor de la imagen mediante una imagen previamente construida.
        /// </summary>
        /// <param name="texture"></param>
        internal ConfettiPaper(Texture2D texture, Vector2 location, Color color, Vector2 scale, Vector2 speed, Vector2 speedIncrement) : base()
        {
            Texture = texture;
            Location = location;
            Color = color;
            Scale = scale;
            Speed = speed;
            SpeedIncrement = speedIncrement;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        internal override void Update(GameTime gameTime)
        {
            Location += Speed;
            Speed += SpeedIncrement;/*Se aumenta la velocidad*/
        }

        internal override void Draw(GameTime gameTime)
        {
            Screen.SpriteBatch.Draw(Texture, Location.Redim(), null, Color * Transparency, Math.Atan2(Speed.X, -Speed.Y).ToSingle(), new(Texture.Width.Half(), Texture.Height.Half()), Scale, SpriteEffects.None, 0f);
        }

        #endregion
    }
}
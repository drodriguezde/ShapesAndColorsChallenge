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
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Effects.Bloom;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class ProgressBarStars : ProgressBar, IDisposable
    {
        #region CONST

        const int STAR_OFFSET = 128;

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        /// <summary>
        /// Estrellas animadas.
        /// </summary>
        Animation[] animatedStar;

        #endregion

        #region PROPERTIES



        #endregion

        #region CONSTRUCTORS

        internal ProgressBarStars(ModalLevel modalLevel, long endValue, Rectangle bounds) : base(modalLevel, endValue, bounds)
        {

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
        internal new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                for (int i = 0; i < animatedStar.Length; i++)
                    animatedStar[i].Dispose();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ProgressBarStars()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            TextureManager.LoadAnimatedStar();
            LoadStars();
        }

        /// <summary>
        /// Obtiene la ubicación de la estrella en pantalla.
        /// </summary>
        /// <param name="starIndex"></param>
        /// <returns></returns>
        Vector2 GetStarLocation(int starIndex, float scale)
        {
            int offset = (STAR_OFFSET * scale).ToInt();
            Increment = EndValue / 100 * GameData.PercentToStar(starIndex);
            int x = RelativeCurrentValue - offset;
            Increment = 0;
            return new Vector2(x, Bounds.Top - offset);
        }

        void LoadStars()
        {
            Vector2 scale = Screen.GetScaleToFit(new Vector2(256, 256), new Vector2(Bounds.Height, Bounds.Height).Redim());

            animatedStar = new Animation[3]
            {
                new Animation(TextureManager.AnimatedStar, Const.ANIMATED_STAR_LOOP, GetStarLocation(0, scale.X).Redim(), scale) { Visible = true, BloomEnabled = true, BloomEnabledOnlyInMotion = true, BloomValues = new BloomValues().SetStarInMotionBloom() },
                new Animation(TextureManager.AnimatedStar, Const.ANIMATED_STAR_LOOP, GetStarLocation(1, scale.X).Redim(), scale) { Visible = true, BloomEnabled = true, BloomEnabledOnlyInMotion = true, BloomValues = new BloomValues().SetStarInMotionBloom() },
                new Animation(TextureManager.AnimatedStar, Const.ANIMATED_STAR_LOOP, GetStarLocation(2, scale.X).Redim(), scale) { Visible = true, BloomEnabled = true, BloomEnabledOnlyInMotion = true, BloomValues = new BloomValues().SetStarInMotionBloom() }
            };
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < animatedStar.Length; i++)
            {
                if (Increment >= GameData.PointsToStar(i, OrchestratorManager.LevelNumber))
                {
                    if (i.IsZero() && !animatedStar[i].Running)
                        animatedStar[i].Start();/*La primera estrella inicia normal*/
                    else
                    {
                        if (animatedStar.First().CurrentFrame == 0 && !animatedStar[i].Running)
                            animatedStar[i].Start(animatedStar.First().LastFrameTime);/*Las siguientes estrellas se sincronizan con la primera para girar igual*/
                    }
                }
                else
                    animatedStar[i].Stop();

                animatedStar[i].Update(gameTime);
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            for (int i = 0; i < animatedStar.Length; i++)
                animatedStar[i].Draw(gameTime);
        }

        #endregion
    }
}
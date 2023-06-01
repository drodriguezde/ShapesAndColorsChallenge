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
using ShapesAndColorsChallenge.Enum;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Particles
{
    internal static class ParticleEngine
    {
        #region VARS

        static long counter = 0;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Cada cuantos frames hay que lanzar más partículas.
        /// </summary>
        static int StartRatioFrames { get; set; } = 100;

        static List<ParticlePack> ParticlePacks = new();

        static bool Running { get; set; }

        static ParticleType ParticleType { get; set; } = ParticleType.Falling;

        #endregion

        #region METHODS

        internal static void Start(ParticleType particleType, int startRatioFrames = 100)
        {
            ParticleType = particleType;
            StartRatioFrames = startRatioFrames;
            Reset();
            Running = true;
        }

        internal static void End()
        {
            Running = false;
            Reset();
        }

        static void Reset()
        {
            ParticlePacks.Clear();
            counter = 0;
        }

        static void DoParticles()
        {
            ParticlePacks.RemoveAll(t => !t.Running);

            if (counter % StartRatioFrames == 0)
            {
                if (ParticleType == ParticleType.Falling)
                    ParticlePacks.Add(new(ParticleType.Falling, ShapeType.Oval, TileColor.Orange, new(Screen.BoundsOffset.X, -Screen.Bounds.Height, Screen.Bounds.Width, Screen.Bounds.Height), new(0, -Screen.Bounds.Height, Screen.Bounds.Width, Screen.Bounds.Height.Double()), true, true, 20));
                else
                {
                    ParticlePacks.Add(new(ParticleType, ShapeType.None, TileColor.None, new(Screen.BoundsOffset.X, 200, BaseBounds.Limits.Width, Screen.Bounds.Height - 600), Screen.Bounds, true, true, 20));
                    StartRatioFrames = Statics.GetRandom(10, 80);
                    counter = 1;
                }

                ParticlePacks.Last().Start();
            }

            counter++;
        }

        internal static void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            DoParticles();

            for (int i = 0; i < ParticlePacks.Count; i++)
                ParticlePacks[i].Update(gameTime);
        }

        internal static void Draw(GameTime gameTime)
        {
            if (!Running)
                return;

            for (int i = 0; i < ParticlePacks.Count; i++)
                ParticlePacks[i].Draw(gameTime);
        }

        #endregion
    }
}

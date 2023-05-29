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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Particles.ConfettiParticle;
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
        /// Cada cuantos frames hay que lanzar más confeti.
        /// </summary>
        static int StartRatioFrames { get; set; } = 100;

        static List<Confetti> Confettis = new();

        static bool Running { get; set; }

        static ConfettiType ConfettiType { get; set; } = ConfettiType.Falling;

        #endregion

        #region METHODS

        internal static void Start(ConfettiType confettiType)
        {
            ConfettiType = confettiType;
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
            Confettis.Clear();
            counter = 0;
        }

        static void DoParticles()
        {
            Confettis.RemoveAll(t => !t.Running);

            if (counter % StartRatioFrames == 0)
            {
                if (ConfettiType == ConfettiType.Falling)
                    Confettis.Add(new(ConfettiType.Falling, ShapeType.Oval, TileColor.Orange, new(0, -BaseBounds.Bounds.Height, BaseBounds.Bounds.Width, BaseBounds.Bounds.Height), new(0, -BaseBounds.Bounds.Height, BaseBounds.Bounds.Width, BaseBounds.Bounds.Height.Double()), true, true, 30));
                else
                {
                    Confettis.Add(new(ConfettiType, ShapeType.None, TileColor.None, new(BaseBounds.Limits.X, 200, BaseBounds.Limits.Width, BaseBounds.Bounds.Height - 600), BaseBounds.Bounds, true, true, 20));
                    StartRatioFrames = Statics.GetRandom(10, 80);
                    SoundManager.PlayBallonPop();
                    counter = 1;
                }

                Confettis.Last().Start();
            }

            counter++;
        }

        internal static void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            DoParticles();

            for (int i = 0; i < Confettis.Count; i++)
                Confettis[i].Update(gameTime);
        }

        internal static void Draw(GameTime gameTime)
        {
            if (!Running)
                return;

            for (int i = 0; i < Confettis.Count; i++)
                Confettis[i].Draw(gameTime);
        }

        #endregion
    }
}

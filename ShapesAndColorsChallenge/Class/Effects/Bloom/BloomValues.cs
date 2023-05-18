using static ShapesAndColorsChallenge.Class.Effects.Bloom.BloomFilter;

namespace ShapesAndColorsChallenge.Class.Effects.Bloom
{
    internal class BloomValues
    {
        #region PREPERTIES

        internal BloomPresets BloomPreset { get; set; } = BloomPresets.Small;

        internal bool HalfResolution { get; set; } = false;

        internal float BloomThreshold { get; set; } = 0f;

        internal float BloomStreakLength { get; set; } = 0f;

        internal float BloomStrengthMultiplier { get; set; } = 0f;

        #endregion

        #region CONSTRUCTORS

        internal BloomValues()
        {

        }

        #endregion

        #region METHODS

        /// <summary>
        /// Establece los valores de Bloom cuando las estrellas están moviendose sobre la barra de progreso.
        /// </summary>
        internal BloomValues SetStarInMotionBloom()
        {
            BloomPreset = BloomPresets.Focussed;
            HalfResolution = false;
            BloomThreshold = 0.20f;
            BloomStreakLength = 1;
            BloomStrengthMultiplier = 0.90f;
            return this;
        }

        #endregion
    }
}

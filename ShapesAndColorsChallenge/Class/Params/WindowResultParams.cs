using ShapesAndColorsChallenge.DataBase.Tables;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class WindowResultParams
    {
        #region PROPERTIES

        internal long Points { get; private set; } = 0;

        internal int Stars { get; private set; } = 0;

        internal bool NewRecord { get; private set; } = false;

        internal Challenge Challenge { get; private set; } = null;

        internal int TilesFinded { get; private set; }

        internal int UserMistakes { get; private set; }

        internal int PowerUpsUsed { get; private set; }

        #endregion

        #region CONSTRUCTOR

        internal WindowResultParams()
        {

        }

        internal WindowResultParams(long points, int stars, bool newRecord, Challenge challenge, int tilesFinded, int userMistakes, int powerUpsUsed)
        {
            Points = points;
            Stars = stars;
            NewRecord = newRecord;
            Challenge = challenge;
            TilesFinded = tilesFinded;
            UserMistakes = userMistakes;
            PowerUpsUsed = powerUpsUsed;
        }

        #endregion
    }
}

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

        internal bool IsChallenge { get; private set; } = false;

        internal bool ChallengeCompleted { get; private set; } = false;

        #endregion

        #region CONSTRUCTOR

        internal WindowResultParams()
        {

        }

        internal WindowResultParams(long points, int stars, bool newRecord, Challenge challenge, bool isChallenge, bool challengeCompleted)
        {
            Points = points;
            Stars = stars;
            NewRecord = newRecord;
            Challenge = challenge;
            IsChallenge = isChallenge;
            ChallengeCompleted = challengeCompleted;
        }

        #endregion
    }
}

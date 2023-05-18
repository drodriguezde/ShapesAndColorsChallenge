using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class WindowResultParams
    {
        #region PROPERTIES

        internal long Points { get; set; } = 0;
        internal int Stars { get; set; } = 0;

        internal bool NewRecord { get; set; } = false;

        #endregion

        #region CONSTRUCTOR

        internal WindowResultParams()
        {

        }

        internal WindowResultParams(long points, int stars, bool newRecord)
        {
            Points = points;
            Stars = stars;
            NewRecord = newRecord;
        }

        #endregion
    }
}

using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class WindowRewardParams
    {
        #region PROPERTIES

        internal PerkType Reward1 { get; set; } = PerkType.None;
        internal PerkType Reward2 { get; set; } = PerkType.None;
        internal PerkType Reward3 { get; set; } = PerkType.None;

        internal int Amount1 { get; set; }
        internal int Amount2 { get; set; }
        internal int Amount3 { get; set; }

        #endregion

        #region CONSTRUCTOR

        internal WindowRewardParams()
        {

        }

        internal WindowRewardParams(PerkType reward1, int amount1, PerkType reward2, int amount2, PerkType reward3, int amount3)
        {
            Reward1 = reward1;
            Reward2 = reward2;
            Reward3 = reward3;
            Amount1 = amount1;
            Amount2 = amount2;
            Amount3 = amount3;
        }

        #endregion
    }
}

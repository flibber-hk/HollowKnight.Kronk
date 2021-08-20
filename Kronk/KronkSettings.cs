using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;

namespace Kronk
{
    public class KronkSettings
    {
        #region Levers
        public int LeversHit = 0;
        public bool MantisRewardsLever = false;
        #endregion

        #region Rocks
        public int RocksBroken = 0;
        #endregion
    }

    public class GlobalSettings
    {
        public bool displayCounter = true;
        public CountingMode countingMode = CountingMode.Levers;
        public CounterPosition counterPosition = CounterPosition.TopRight;
    }

    public enum CountingMode
    {
        Levers = 0,
        Rocks
    }

    public enum CounterPosition
    {
        TopRight = 0,
        BottomLeft,
        BottomRight,
        AboveHUD,
        BelowHUD
    }
}

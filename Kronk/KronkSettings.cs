using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;

namespace Kronk
{
    public class KronkSettings : ModSettings
    {

        public int LeversHit
        {
            get => GetInt(0);
            set => SetInt(value);
        }

        public bool MantisRewardsLever
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public int RocksBroken
        {
            get => GetInt(0);
            set => SetInt(value);
        }

    }

    public class GlobalSettings : ModSettings
    {
        public int countingMode
        {
            get => GetInt(0);
            set => SetInt(value);
        }
        public int counterPosition
        {
            get => GetInt(0);
            set => SetInt(value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kronk.Randomizer;

namespace Kronk
{
    public static class RockCount
    {
        internal const int NUMROCKS = 207;

        public static void Hook()
        {
            On.PlayMakerFSM.OnEnable += CountRocks;
        }


        private static void CountRocks(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.FsmName != "Geo Rock") return;

            self.GetState("Destroy").AddFirstAction(new ExecuteLambda(() =>
            {
                IncrementRockCount();
            }));
        }


        private static void IncrementRockCount()
        {
            Kronk.instance.Settings.RocksBroken += 1;
            Display.UpdateText();

            if (Kronk.instance.Settings.RocksBroken == NUMROCKS && Kronk.instance.countingMode == Kronk.CountingMode.Rocks)
            {
                Kronk.SendMessageToLivesplit();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kronk.Randomizer;

namespace Kronk.Counters
{
    public static class RockCount
    {
        internal const int NUMOBJECTS = 207;
        public static void Hook()
        {
            Kronk.instance.Log("Hooking Rock Count...");
            On.PlayMakerFSM.OnEnable += CountRocks;
        }
        private static bool IsActive => Kronk.globalSettings.countingMode == CountingMode.Rocks;

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
            Kronk.localSettings.RocksBroken += 1;

            if (IsActive)
            {
                Display.UpdateText();
                if (Kronk.localSettings.RocksBroken == NUMOBJECTS)
                {
                    Kronk.SendMessageToLivesplit();
                }
            }
        }

    }
}

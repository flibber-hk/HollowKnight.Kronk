using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kronk.Util;

namespace Kronk.Counters
{
    public static class TotemCount
    {
        internal const int NUMOBJECTS = 59;

        public static void Hook()
        {
            Kronk.instance.Log("Hooking Totem Count...");
            On.PlayMakerFSM.OnEnable += CountTotems;
        }
        private static bool IsActive => Kronk.globalSettings.countingMode == CountingMode.Totems;

        private static void CountTotems(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName != "soul_totem") return;

            self.GetState("Hit").AddFirstAction(new ExecuteLambda(() => IncrementTotemCount(self.gameObject.name, self.gameObject.scene.name)));
        }
        private static void IncrementTotemCount(string gameObject, string scene)
        {
            (string, string) key = (gameObject, scene);
            if (Kronk.localSettings.TotemsHit.Contains(key)) return;

            Kronk.localSettings.TotemsHit.Add(key);

            if (IsActive)
            {
                Display.UpdateText();
                if (Kronk.localSettings.TotemCount == NUMOBJECTS)
                {
                    Kronk.SendMessageToLivesplit();
                }
            }
        }
    }
}

using System.Collections.Generic;
using Kronk.Util;

namespace Kronk.Counters
{
    public static class RockCount
    {
        public static readonly HashSet<string> BadScenes = new HashSet<string>() { "Crossroads_ShamanTemple", "Abyss_06_Core" };

        internal const int NUMOBJECTS = 207;
        public static void Hook()
        {
            Kronk.instance.Log("Hooking Rock Count...");
            Hooks.OnFsmEnable += CountRocks;
        }
        private static bool IsActive => Kronk.globalSettings.countingMode == CountingMode.Rocks;

        private static void CountRocks(PlayMakerFSM fsm)
        {
            if (fsm.FsmName != "Geo Rock") return;

            fsm.GetState("Destroy").AddFirstAction(new ExecuteLambda(() =>
            {
                IncrementRockCount(fsm);
            }));
        }


        private static void IncrementRockCount(PlayMakerFSM fsm)
        {
            if (BadScenes.Contains(fsm.gameObject.scene.name))
            {
                (string, string, bool) key = (fsm.gameObject.scene.name, fsm.gameObject.name, fsm.transform.parent != null);
                if (!Kronk.localSettings.DupableRocksBroken.Add(key)) return;
            }

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

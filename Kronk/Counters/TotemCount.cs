using Kronk.Util;

namespace Kronk.Counters
{
    public static class TotemCount
    {
        internal const int NUMOBJECTS = 59;

        public static void Hook()
        {
            Kronk.instance.Log("Hooking Totem Count...");
            Hooks.OnFsmEnable += CountTotems;
        }
        private static bool IsActive => Kronk.globalSettings.countingMode == CountingMode.Totems;

        private static void CountTotems(PlayMakerFSM fsm)
        {
            if (fsm.FsmName != "soul_totem") return;

            fsm.GetState("Hit").AddFirstAction(new ExecuteLambda(() => IncrementTotemCount(fsm.gameObject.name, fsm.gameObject.scene.name)));
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

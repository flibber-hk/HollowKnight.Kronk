using Kronk.Util;

namespace Kronk.Counters
{
    public static class RockCount
    {
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

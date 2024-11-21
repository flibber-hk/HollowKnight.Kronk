using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.SceneManagement;
using Kronk.Util;

namespace Kronk.Counters
{
    public static class LeverCount
    {
        internal const int NUMOBJECTS = 63;
        public static void Hook()
        {
            Kronk.instance.Log("Hooking Lever Count...");
            Hooks.OnFsmEnable += CountLevers;
            On.BridgeLever.OpenBridge += CountBridgeLevers;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CountMantisLever;
        }
        private static bool IsActive => Kronk.globalSettings.countingMode == CountingMode.Levers;

        private static void CountLevers(PlayMakerFSM fsm)
        {
            if (!(fsm.FsmName == "Switch Control" || fsm.FsmName == "toll switch"))
            {
                return;
            }

            // Exclude Godhome orb from count
            if (fsm.gameObject.name == "gg_roof_lever") return;

            if (fsm.GetState("Hit") is FsmState hitState)
            {
                hitState.AddFirstAction(new ExecuteLambda(() =>
                {
                    IncrementLeverCount();
                }));
            }
        }
        private static IEnumerator CountBridgeLevers(On.BridgeLever.orig_OpenBridge orig, BridgeLever self)
        {
            IncrementLeverCount();

            return orig(self);
        }
        private static void CountMantisLever(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Fungus2_31"
                && !Kronk.localSettings.MantisRewardsLever
                && PlayerData.instance.defeatedMantisLords)
            {
                Kronk.localSettings.MantisRewardsLever = true;
                IncrementLeverCount();
            }
        }

        private static void IncrementLeverCount()
        {
            Kronk.localSettings.LeversHit += 1;

            if (IsActive)
            {
                Display.UpdateText();
                if (Kronk.localSettings.LeversHit == NUMOBJECTS)
                {
                    Kronk.SendMessageToLivesplit();
                }
            }
        }
    }
}

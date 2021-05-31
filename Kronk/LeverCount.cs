using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using UnityEngine.SceneManagement;
using Kronk.Randomizer;

namespace Kronk
{
    public static class LeverCount
    {
        internal const int NUMLEVERS = 63;

        public static void Hook()
        {
            On.PlayMakerFSM.OnEnable += CountLevers;
            On.BridgeLever.OpenBridge += CountBridgeLevers;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CountMantisLever;
        }


        private static void CountLevers(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (!(self.FsmName == "Switch Control" || self.FsmName == "toll switch"))
            {
                return;
            }

            // Exclude Godhome orb from count
            if (self.gameObject.name == "gg_roof_lever") return;

            if (self.GetState("Hit") is FsmState hitState)
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
            if (!string.IsNullOrEmpty(arg0.name)
                && GameManager.GetBaseSceneName(arg0.name) == "Fungus2_15"
                && arg1.name == "Fungus2_31"
                && !Kronk.instance.Settings.MantisRewardsLever
                && PlayerData.instance.defeatedMantisLords)
            {
                Kronk.instance.Settings.MantisRewardsLever = true;
                IncrementLeverCount();
            }
        }

        private static void IncrementLeverCount()
        {
            Kronk.instance.Settings.LeversHit += 1;
            Display.UpdateText();

            if (Kronk.instance.Settings.LeversHit == NUMLEVERS && Kronk.instance.countingMode == Kronk.CountingMode.Levers)
            {
                Kronk.SendMessageToLivesplit();
            }
        }
    }
}

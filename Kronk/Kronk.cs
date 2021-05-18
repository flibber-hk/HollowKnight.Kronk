using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using Modding;
using Kronk.Randomizer;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Kronk
{
    public class Kronk : Mod
    {
        internal const int NUMLEVERS = 63;

        internal static Kronk instance;

        public KronkSettings Settings = new KronkSettings();
        public override ModSettings SaveSettings 
        {
            get => Settings;
            set => Settings = (KronkSettings)value;
        }



        public override void Initialize()
        {
            instance = this;

            On.PlayMakerFSM.OnEnable += CountLevers;
            On.BridgeLever.OpenBridge += CountBridgeLevers;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CountMantisLever;

            CanvasUtil.CreateFonts();

            LeverDisplay.Hook();
        }


        private void CountLevers(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
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
        private IEnumerator CountBridgeLevers(On.BridgeLever.orig_OpenBridge orig, BridgeLever self)
        {
            IncrementLeverCount();

            return orig(self);
        }
        private static void CountMantisLever(Scene arg0, Scene arg1)
        {
            if (!string.IsNullOrEmpty(arg0.name)
                && GameManager.GetBaseSceneName(arg0.name) == "Fungus2_15"
                && arg1.name == "Fungus2_31"
                && !instance.Settings.MantisRewardsLever
                && PlayerData.instance.defeatedMantisLords)
            {
                instance.Settings.MantisRewardsLever = true;
                IncrementLeverCount();
            }
        }

        private static void IncrementLeverCount()
        {
            instance.Settings.LeversHit += 1;
            LeverDisplay.UpdateText();

            if (instance.Settings.LeversHit == NUMLEVERS)
            {
                SendMessageToLivesplit();
            }
        }

        // Setting the Hunter's Mark playerdata for 0.1s so that Livesplit has a chance to autosplit on the last lever
        private static void SendMessageToLivesplit()
        {
            IEnumerator toggleMark()
            {
                bool temp = PlayerData.instance.killedHunterMark;
                PlayerData.instance.SetBool(nameof(PlayerData.killedHunterMark), true);
                yield return new WaitForSeconds(0.1f);
                PlayerData.instance.SetBool(nameof(PlayerData.killedHunterMark), temp);
            }
            GameManager.instance.StartCoroutine(toggleMark());
        }

        public override string GetVersion()
        {
            return "0.3s";
        }

    }
}

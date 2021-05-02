using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using Modding;
using Kronk.Randomizer;

namespace Kronk
{
    public class Kronk : Mod
    {
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

            LeverDisplay.Hook();
        }

        private void CountLevers(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.FsmName != "Switch Control")
            {
                return;
            }

            // Exclude Godhome orb from count
            if (self.gameObject.name == "gg_roof_lever") return;

            self.GetState("Hit").AddFirstAction(new ExecuteLambda(() =>
            {
                Settings.LeversHit += 1;
                LeverDisplay.UpdateText();
            }));
        }

        public override string GetVersion()
        {
            return "0.1";
        }

    }
}

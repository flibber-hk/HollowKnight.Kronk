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

            ModHooks.Instance.LanguageGetHook += ShowLeversInInventory;
        }

        private string ShowLeversInInventory(string key, string sheetTitle)
        {
            if (key == "INV_DESC_SPELL_FOCUS" && sheetTitle == "UI")
            {
                return $"Hit {Settings.LeversHit} Levers";
            }

            return Language.Language.GetInternal(key, sheetTitle);
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

            self.GetState("Hit").AddFirstAction(new ExecuteLambda(() => Settings.LeversHit += 1));
        }

        public override string GetVersion()
        {
            return "0.1";
        }

    }
}
